using System.Text.RegularExpressions;

namespace MiniTemplateEngine;

public class HtmlTemplateRenderer : IHtmlTemplateRenderer
{
    private static readonly string[] Operators =
    {
        ">=", "<=", "==", "!=",
        ">", "<"
    };
    private readonly string regExpr = @"\$\{\S*\}";
    private readonly string ifRegExpr = @"\$(?:if\s+\(|else|end(?!\w))";
    private readonly string foreachRegExpr = @"\$(?:foreach\s+\((\w+)\s+in\s+([^)]+)\)|endforeach)";

    
    public string RenderFromString(string htmlTemplate, object dataModel)
    {
        var context = new TemplateContext(dataModel);

        return RenderFromString(htmlTemplate, context);
    }

    private string RenderFromString(string htmlTemplate, TemplateContext context)
    {
        htmlTemplate = HandleForeach(context, htmlTemplate);
        htmlTemplate = HandleIf(context, htmlTemplate);
        htmlTemplate = Regex.Replace(htmlTemplate, @"\r?\n[ \t]*\r?\n", "\n"); // убираем проблемные 

        var result = ReplacePlaceholders(context, htmlTemplate);

        return result;
    }

    private string ReplacePlaceholders(TemplateContext templateContext, string htmlTemplate)
    {
        return Regex.Replace(htmlTemplate, regExpr, match =>
        {
            string path = match.Value.TrimStart('$')
                .TrimStart('{')
                .TrimEnd('}');

            object value = GetPropertyValue(templateContext, path);
            return value.ToString() ?? string.Empty;
        });
    }

    public string RenderFromFile(string filePath, object dataModel)
    {
        if (!File.Exists(filePath))
        {
            throw new Exception("Нет файла по такому пути");
        }
        string htmlTemplate = File.ReadAllText(filePath);
        return RenderFromString(htmlTemplate, dataModel);
    }

    public string RenderToFile(string inputFilePath, string outputFilePath, object dataModel)
    {
        if (!File.Exists(outputFilePath))
        {
            File.Create(outputFilePath);
        }

        string result = RenderFromFile(inputFilePath, dataModel);
        File.WriteAllText(outputFilePath, result);
        return result;
    }

    private object GetPropertyValue(object datamodel, string path)
    {
        object currentValue = datamodel;
        foreach (var propertyName in path.Split('.'))
        {
            var property = currentValue.GetType().GetProperty(propertyName);
            if (property == null)
            {
                throw new InvalidOperationException(
                    $"Свойство '{propertyName}' не найдено в типе '{currentValue.GetType().Name}'.");
            }

            currentValue = property.GetValue(currentValue);
        }

        return currentValue;
    }
    private object GetPropertyValue(TemplateContext context, string path)
    {
        string[] properties = path.Split('.');

        object currentValue = null;
        int startIndex = 0;

        // Ищем переменную в текущем и родительских контекстах
        TemplateContext currentContext = context;
        bool variableFound = false;

        while (currentContext != null)
        {
            if (currentContext.VariableName == properties[0])
            {
                currentValue = currentContext.VariableValue;
                startIndex = 1;
                variableFound = true;
                break;
            }

            currentContext = currentContext.Parent;
        }

        // Если переменная не найдена, ищем свойство исходной модели
        if (!variableFound)
        {
                currentValue = context.DataModel;
        }

        // Получаем оставшиеся свойства через рефлексию
        for (int i = startIndex; i < properties.Length; i++)
        {
            var property = currentValue.GetType().GetProperty(properties[i]);

            if (property == null)
            {
                throw new InvalidOperationException(
                    $"Свойство '{properties[i]}' не найдено в типе '{currentValue.GetType().Name}'.");
            }

            currentValue = property.GetValue(currentValue);
        }

        return currentValue;
    }
    
    
    private string HandleIf(TemplateContext context, string template)
    {
        var matches = Regex.Matches(template, ifRegExpr);

        for (int i = 0; i < matches.Count; i++)
        {
            var m = matches[i];
            var value = m.Value.TrimStart('$');

            if (value.StartsWith("if "))
            {
                int conditionStart = m.Index + m.Length;
                int conditionEnd = FindConditionEnd(template, conditionStart);

                string condition = template.Substring(
                    conditionStart,
                    conditionEnd - conditionStart
                );
                bool conditionResult = EvaluateCondition(context, condition);

                var (elseIndex, endIndex) = FindElsesAndEnds(matches, i);

                int start = conditionEnd + 1;

                string trueBranch;
                string elseBranch = "";

                if (endIndex == -1)
                {
                    throw new InvalidOperationException("Не найден $end для $if");
                }
                if (elseIndex != -1)
                {
                    int ifLength = elseIndex - start;
                    trueBranch = template.Substring(start, ifLength);

                    int elseLength = endIndex - (elseIndex + "$else".Length);
                    elseBranch = template.Substring(
                        elseIndex + "$else".Length,
                        elseLength
                    );
                }
                else
                {
                    int ifLength = endIndex - start;
                    trueBranch = template.Substring(start, ifLength);
                }

                string selectedBranch;

                if (conditionResult)
                    selectedBranch = trueBranch;
                else
                    selectedBranch = elseBranch;
                int blockLength = endIndex + "$end".Length - m.Index;

                string before = template.Substring(0, m.Index);
                string after = template.Substring(m.Index + blockLength);

                template = before + selectedBranch + after;
                return HandleIf(context, template);
            }
        }

        return template;
    }

    private string HandleForeach(TemplateContext context, string template)
    {
        var matches = Regex.Matches(template, foreachRegExpr);
        
        for (int i = 0; i < matches.Count; i++)
        {
            var m = matches[i];
            var value = m.Value.TrimStart('$');
            if (value.StartsWith("foreach "))
            {
                string variableName = m.Groups[1].Value;
                string collectionName = m.Groups[2].Value;

                int endIndex = FindForeachEnd(matches, i);
                if (endIndex == -1)
                    throw new InvalidOperationException(
                        "Не найден $endforeach для $foreach.");
                int start = m.Index + m.Length;
                string body = template.Substring(start, endIndex - start);
                object collection = GetPropertyValue(context, collectionName);
                if (collection is not System.Collections.IEnumerable items)
                    throw new InvalidOperationException(
                        $"'{collectionName}' не является коллекцией.");
                string result = "";
                foreach (object item in items)
                {
                    var childContext = new TemplateContext(
                        context.DataModel,
                        variableName,
                        item,
                        context
                    );

                    string renderedItem = RenderFromString(body, childContext);

                    result += renderedItem;
                }
                int endTagLength = 0;

                for (int j = i + 1; j < matches.Count; j++)
                {
                    if (matches[j].Index == endIndex)
                    {
                        endTagLength = matches[j].Length;
                        break;
                    }
                }
                
                int blockLength = endIndex + endTagLength - m.Index;

                string before = template.Substring(0, m.Index);
                string after = template.Substring(m.Index + blockLength);

                template = before + result + after;

                return HandleForeach(context, template);
                
            }
        }
        return template;
    }

    private static int FindForeachEnd(MatchCollection matches, int foreachIndex)
    {
        int depth = 1;
        int endIndex = -1;
        for (int i = foreachIndex + 1; i < matches.Count; i++)
        {
            var m = matches[i];
            var value = m.Value.TrimStart('$');
            if (value.StartsWith("foreach "))
            {
                depth++;
            }

            if (value.Equals("endforeach"))
            {
                depth--;
                if (depth == 0)
                {
                    endIndex = m.Index;
                    break;
                }
            }
        }
        return endIndex;
    }

    private static (int, int) FindElsesAndEnds(MatchCollection matches, int ifIndex)
    {
        int depth = 1;
        int elseIndex = -1;
        int endIndex = -1;
        for (int i = ifIndex+1; i < matches.Count; i++)
        {
            var m = matches[i];
            var value = m.Value.TrimStart('$');
            if (value.StartsWith("if "))
            {
                depth++;
            }

            if (value.Equals("else"))
            {
                if (depth == 1) elseIndex = m.Index;
            }

            if (value.Equals("end"))
            {
                depth--;
                if (depth == 0)
                {
                    endIndex = m.Index;
                    break;
                }
            }
        }
        return (elseIndex, endIndex);
    }
    
    private int FindConditionEnd(string template, int startIndex)
    {
        int depth = 1;

        for (int i = startIndex; i < template.Length; i++)
        {
            if (template[i] == '(')
                depth++;
            else if (template[i] == ')')
                depth--;

            if (depth == 0)
                return i;
        }

        throw new InvalidOperationException("Не найдена закрывающая скобка для $if");
    }
    
    private bool EvaluateCondition(TemplateContext context, string condition)
    {
        condition = condition.Trim();

        while (HasWrappingParentheses(condition))
        {
            condition = condition.Substring(1, condition.Length - 2).Trim();
        }
        int index = FindTopLevelOperator(condition, "||");

        if (index != -1)
        {
            string left = condition.Substring(0, index).Trim();
            string right = condition.Substring(index + "||".Length).Trim();

            return EvaluateCondition(context, left)
                   || EvaluateCondition(context, right);
        }
        
        index = FindTopLevelOperator(condition, "&&");

        if (index != -1)
        {
            string left = condition.Substring(0, index).Trim();
            string right = condition.Substring(index + 2).Trim();

            return EvaluateCondition(context, left)
                   && EvaluateCondition(context, right);
        }
        
        if (condition.StartsWith("!"))
        {
            string innerCondition = condition.Substring(1).Trim();
            return !EvaluateCondition(context, innerCondition);
        }

        string[] parts = SplitCondition(condition);
        // для bool свойств
        if (parts.Length == 1)
        {
            object value = GetPropertyValue(context, condition);
        
            if (value is bool result)
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        
        string propertyName = parts[0];
        string operation = parts[1];
        string expectedValue = parts[2];
        
        object propertyValue = GetPropertyValue(context, propertyName);
        if (propertyValue is int number &&
            int.TryParse(expectedValue, out int expectedNumber))
        {
            return CompareNumbers(number, expectedNumber, operation);
        }

        if (propertyValue is string text && (operation.Equals("==") || operation.Equals("!=")))
        {
            string expectedText = expectedValue.Trim('"');
            return CompareStrings(text, expectedText, operation);
        }
        throw new InvalidOperationException(
            $"Не удалось сравнить '{propertyName}' с '{expectedValue}'.");
    }
    
    private bool HasWrappingParentheses(string condition)
    {
        if (!condition.StartsWith("(") || !condition.EndsWith(")"))
            return false;

        int depth = 0;

        for (int i = 0; i < condition.Length; i++)
        {
            if (condition[i] == '(')
                depth++;
            else if (condition[i] == ')')
                depth--;

            // Внешняя пара закрылась раньше конца выражения
            if (depth == 0 && i < condition.Length - 1)
                return false;
        }

        return depth == 0;
    }
    
    private int FindTopLevelOperator(string condition, string oper)
    {
        int depth = 0;
        bool insideQuotes = false;

        for (int i = 0; i <= condition.Length - oper.Length; i++)
        {
            if (condition[i] == '"')
                insideQuotes = !insideQuotes;

            if (insideQuotes)
                continue;

            if (condition[i] == '(')
                depth++;
            else if (condition[i] == ')')
                depth--;

            if (depth == 0 &&
                condition.Substring(i, oper.Length) == oper)
            {
                return i;
            }
        }

        return -1;
    }

    private bool CompareStrings(string text, string expectedText, string operation)
    {
        return operation switch
        {
            "==" => text.Equals(expectedText),
            "!=" => !text.Equals(expectedText)
        };
    }
    private bool CompareNumbers(int number, int expected, string operation)
    {
        return operation switch
        {
            "==" => number == expected,
            "!=" => number != expected,
            ">"  => number > expected,
            "<"  => number < expected,
            ">=" => number >= expected,
            "<=" => number <= expected,
            _ => throw new InvalidOperationException(
                $"Неизвестный оператор: {operation}")
        };
    }

    private string[] SplitCondition(string condition)
    {
        foreach (var oper in Operators)
        {
            int index = condition.IndexOf(oper);
            if (index != -1)
            {
                string left = condition.Substring(0, index).Trim();
                string right = condition.Substring(index + oper.Length).Trim();
                
                return new[] { left, oper, right };
            }
        }
        return new[] { condition.Trim() }; 
    }

}