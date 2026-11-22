namespace MiniTemplateEngine;

public class TemplateContext
{
    public string VariableName { get; set; }
    public object VariableValue { get; set; }
    public TemplateContext Parent { get; set; }
    public object DataModel { get; set; }

    public TemplateContext(
        object dataModel,
        string variableName = null,
        object variableValue = null,
        TemplateContext parent = null)
    {
        DataModel = dataModel;
        VariableName = variableName;
        VariableValue = variableValue;
        Parent = parent;
    }
}