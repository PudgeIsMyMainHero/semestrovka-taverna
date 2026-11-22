namespace MiniTemplateEngine.UnitTests;

[TestClass]
public class HtmlTemplateRendererTests
{ 
    [TestMethod]
    public void RenderFromString_ReplacingPlaceholders_Positive_ReturnValid1()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();
        // Act
        string template = """
                          <p>Имя: ${Name}</p>
                          <p>Возраст: ${Age}</p>
                          <p>Группа: ${Group}</p>
                          <p>Пол: ${Sex}</p>
                          """;
        object user = new { Name = "Ваня", Group = "11-541", Age = 22, Sex = true };
        string result = templater.RenderFromString(template, user);
        string expected = """
                          <p>Имя: Ваня</p>
                          <p>Возраст: 22</p>
                          <p>Группа: 11-541</p>
                          <p>Пол: True</p>
                          """;
                // Assert
                Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public void RenderFromString_ReplacingPlaceholders_Positive_ReturnValid2()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();
        string template = """
                          <h1>${Name} (${Price})</h1>
                          <p>Название ручки: ${Name}</p>
                          <p>Цвет: ${Color}</p>
                          <p>Ширина: ${Width} мм</p>
                          <p>Цена: ${Price} рублей</p>
                          """;
        string expected = """
                          <h1>Berlingo (100)</h1>
                          <p>Название ручки: Berlingo</p>
                          <p>Цвет: Green</p>
                          <p>Ширина: 1 мм</p>
                          <p>Цена: 100 рублей</p>
                          """;
        object pen = new { Name = "Berlingo", Price = 100, Color = "Green", Width = 1 };
        
        // Act
        string result = templater.RenderFromString(template, pen);
        
        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [TestMethod]
    public void RenderFromString_Foreach_Positive_ReturnValid1()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          <ul>
                          $foreach (pen in Pens)
                          <li>${pen.Name} - ${pen.Color}</li>
                          $endforeach
                          </ul>
                          """;

        object data = new
        {
            Pens = new[]
            {
                new { Name = "Berlingo", Color = "Green" },
                new { Name = "Parker", Color = "Black" },
                new { Name = "Bic", Color = "Blue" }
            }
        };

        string expected = """
                          <ul>
                          <li>Berlingo - Green</li>
                          <li>Parker - Black</li>
                          <li>Bic - Blue</li>
                          </ul>
                          """;

        // Act
        string result = templater.RenderFromString(template, data);
        expected = expected.Replace("\r\n", "\n");
        result = result.Replace("\r\n", "\n"); // иишка сказала, что у нас не нормализованы переносы

        // Assert
        Assert.AreEqual(expected, result);
    }
    [TestMethod]
    public void RenderFromString_Foreach_EmptyCollection_ReturnValid()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          <ul>
                          $foreach (pen in Pens)
                          <li>${pen.Name}</li>
                          $endforeach
                          </ul>
                          """;

        object data = new
        {
            Pens = Array.Empty<object>()
        };

        string expected = """
                          <ul>
                          </ul>
                          """;

        // Act
        string result = templater.RenderFromString(template, data);
        expected = expected.Replace("\r\n", "\n");
        result = result.Replace("\r\n", "\n");

        // Assert
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void RenderFromString_Foreach_NestedForeach_ReturnValid()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $foreach (faculty in Faculties)
                          <h2>${faculty.Name}</h2>
                          $foreach (student in faculty.Students)
                          <p>${student.Name} - ${faculty.Name}</p>
                          $endforeach
                          $endforeach
                          """;

        object data = new
        {
            Faculties = new[]
            {
                new
                {
                    Name = "ИТИС",
                    Students = new[]
                    {
                        new { Name = "Арсений" },
                        new { Name = "Алексений" }
                    }
                },
                new
                {
                    Name = "ИДИПИ",
                    Students = new[]
                    {
                        new { Name = "Марина" }
                    }
                }
            }
        };
        string expected = """
                          <h2>ИТИС</h2>
                          <p>Арсений - ИТИС</p>
                          <p>Алексений - ИТИС</p>
                          <h2>ИДИПИ</h2>
                          <p>Марина - ИДИПИ</p>
                          """;
        // Act
        string result = templater.RenderFromString(template, data);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [TestMethod]
    public void RenderFromString_Foreach_NestedProperties_ReturnValid()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $foreach (pen in Pens)
                          <p>${pen.Name}</p>
                          <p>${pen.Manufacturer.Name}</p>
                          <p>${pen.Manufacturer.Country.Name}</p>
                          $endforeach
                          """;

        object data = new
        {
            Pens = new[]
            {
                new
                {
                    Name = "Berlingo",
                    Manufacturer = new
                    {
                        Name = "Berlingo Group",
                        Country = new { Name = "Франция" }
                    }
                },
                new
                {
                    Name = "Parker",
                    Manufacturer = new
                    {
                        Name = "Parker Pen Company",
                        Country = new { Name = "США" }
                    }
                }
            }
        };

        string expected = """
                          <p>Berlingo</p>
                          <p>Berlingo Group</p>
                          <p>Франция</p>
                          <p>Parker</p>
                          <p>Parker Pen Company</p>
                          <p>США</p>
                          """;

        // Act
        string result = templater.RenderFromString(template, data);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    
    [TestMethod]
    public void RenderFromString_Foreach_ThreeNestedLevels_ReturnValid()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $foreach (faculty in Faculties)
                          <h2>${faculty.Name}</h2>
                          $foreach (group in faculty.Groups)
                          <h3>${group.Name}</h3>
                          $foreach (student in group.Students)
                          <p>${student.Name}</p>
                          $endforeach
                          $endforeach
                          $endforeach
                          """;

        object data = new
        {
            Faculties = new[]
            {
                new
                {
                    Name = "ИТИС",
                    Groups = new[]
                    {
                        new
                        {
                            Name = "11-541",
                            Students = new[]
                            {
                                new { Name = "Арсений" },
                                new { Name = "Алексений" }
                            }
                        },
                        new
                        {
                            Name = "12-542",
                            Students = new[]
                            {
                                new { Name = "Марина" }
                            }
                        }
                    }
                }
            }
        };

        string expected = """
                          <h2>ИТИС</h2>
                          <h3>11-541</h3>
                          <p>Арсений</p>
                          <p>Алексений</p>
                          <h3>12-542</h3>
                          <p>Марина</p>
                          """;

        // Act
        string result = templater.RenderFromString(template, data);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    
    [TestMethod]
    public void RenderFromString_Foreach_SameVariableNames_ReturnValid()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $foreach (item in Faculties)
                          <h2>${item.Name}</h2>
                          $foreach (item in item.Students)
                          <p>${item}</p>
                          $endforeach
                          <h3>${item.Name}</h3>
                          $endforeach
                          """;

        object data = new
        {
            Faculties = new[]
            {
                new
                {
                    Name = "ИТИС",
                    Students = new[] { "Арсений", "Алексений" }
                },
                new
                {
                    Name = "ИДИПИ",
                    Students = new[] { "Марина" }
                }
            }
        };

        string expected = """
                          <h2>ИТИС</h2>
                          <p>Арсений</p>
                          <p>Алексений</p>
                          <h3>ИТИС</h3>
                          <h2>ИДИПИ</h2>
                          <p>Марина</p>
                          <h3>ИДИПИ</h3>
                          """;

        // Act
        string result = templater.RenderFromString(template, data);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    // IF-ELSE ТЕСТЫ
    
    [TestMethod]
    public void RenderFromString_If_TrueCondition_ReturnValid()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $if (IsAuthorized)
                          <p>Доступ разрешён</p>
                          $else
                          <p>Доступ запрещён</p>
                          $end
                          """;

        object user = new { IsAuthorized = true };

        string expected = """
                          <p>Доступ разрешён</p>
                          """;

        // Act
        string result = templater.RenderFromString(template, user);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    
    [TestMethod]
    public void RenderFromString_If_FalseCondition_ReturnValid()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $if (IsAuthorized)
                          <p>Доступ разрешён</p>
                          $else
                          <p>Доступ запрещён</p>
                          $end
                          """;

        object user = new { IsAuthorized = false };

        string expected = """
                          <p>Доступ запрещён</p>
                          """;

        // Act
        string result = templater.RenderFromString(template, user);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }

    
    [TestMethod]
    public void RenderFromString_If_FalseConditionWithoutElse_ReturnValid()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          <h1>Профиль</h1>
                          $if (IsAdmin)
                          <button>Удалить пользователя</button>
                          $end
                          <p>Информация о пользователе</p>
                          """;

        object user = new { IsAdmin = false };

        string expected = """
                          <h1>Профиль</h1>
                          <p>Информация о пользователе</p>
                          """;

        // Act
        string result = templater.RenderFromString(template, user);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    
    [DataTestMethod]
    [DataRow(20, ">", true)]
    [DataRow(10, ">", false)]
    [DataRow(10, "<", true)]
    [DataRow(20, "<", false)]
    [DataRow(18, ">=", true)]
    [DataRow(17, ">=", false)]
    [DataRow(18, "<=", true)]
    [DataRow(19, "<=", false)]
    [DataRow(18, "==", true)]
    [DataRow(19, "==", false)]
    [DataRow(18, "!=", false)]
    [DataRow(19, "!=", true)]
    public void RenderFromString_If_NumericComparison_ReturnValid(
        int age, string operation, bool expectedCondition)
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = $"""
                           $if (Age {operation} 18)
                           True
                           $else
                           False
                           $end
                           """;

        object user = new { Age = age };

        string expected = expectedCondition ? "True" : "False";

        // Act
        string result = templater.RenderFromString(template, user);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [DataTestMethod]
    [DataRow("Admin", "Admin", "==", true)]
    [DataRow("Admin", "User", "==", false)]
    [DataRow("Admin", "User", "!=", true)]
    [DataRow("Admin", "Admin", "!=", false)]
    [DataRow("Hello World", "Hello World", "==", true)]
    public void RenderFromString_If_StringComparison_ReturnValid(
        string role, string expectedRole, string operation, bool expectedCondition)
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = $"""
                           $if (Role {operation} "{expectedRole}")
                           True
                           $else
                           False
                           $end
                           """;

        object user = new { Role = role };

        string expected = expectedCondition ? "True" : "False";

        // Act
        string result = templater.RenderFromString(template, user);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [DataTestMethod]
    [DataRow(true, true, "&&", true)]
    [DataRow(true, false, "&&", false)]
    [DataRow(false, true, "&&", false)]
    [DataRow(false, false, "&&", false)]
    [DataRow(true, true, "||", true)]
    [DataRow(true, false, "||", true)]
    [DataRow(false, true, "||", true)]
    [DataRow(false, false, "||", false)]
    public void RenderFromString_If_LogicalOperators_ReturnValid(
        bool isAdmin, bool isActive, string operation, bool expectedCondition)
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = $"""
                           $if (IsAdmin {operation} IsActive)
                           True
                           $else
                           False
                           $end
                           """;

        object user = new
        {
            IsAdmin = isAdmin,
            IsActive = isActive
        };

        string expected = expectedCondition ? "True" : "False";

        // Act
        string result = templater.RenderFromString(template, user);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [DataTestMethod]
    [DataRow(true, false)]
    [DataRow(false, true)]
    public void RenderFromString_If_LogicalNot_ReturnValid(
        bool isAdmin, bool expectedCondition)
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $if (!IsAdmin)
                          True
                          $else
                          False
                          $end
                          """;

        object user = new { IsAdmin = isAdmin };

        string expected = expectedCondition ? "True" : "False";

        // Act
        string result = templater.RenderFromString(template, user);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    
    [DataTestMethod]
    [DataRow(true, true)]
    [DataRow(false, false)]
    public void RenderFromString_If_DoubleLogicalNot_ReturnValid(
        bool isAdmin, bool expectedCondition)
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $if (!!IsAdmin)
                          True
                          $else
                          False
                          $end
                          """;

        object user = new { IsAdmin = isAdmin };

        string expected = expectedCondition ? "True" : "False";

        // Act
        string result = templater.RenderFromString(template, user);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [DataTestMethod]
    [DataRow(true, false, false, true)]
    [DataRow(false, true, true, true)]
    [DataRow(false, true, false, false)]
    [DataRow(true, true, false, true)]
    public void RenderFromString_If_LogicalOperatorPriority_ReturnValid(
        bool a, bool b, bool c, bool expectedCondition)
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $if (A || B && C)
                          True
                          $else
                          False
                          $end
                          """;

        object data = new { A = a, B = b, C = c };

        string expected = expectedCondition ? "True" : "False";

        // Act
        string result = templater.RenderFromString(template, data);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }

    
    [DataTestMethod]
    [DataRow(true, false, true, true)]
    [DataRow(false, true, true, true)]
    [DataRow(true, true, false, false)]
    [DataRow(false, false, true, false)]
    public void RenderFromString_If_Parentheses_ReturnValid(
        bool a, bool b, bool c, bool expectedCondition)
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $if ((A || B) && C)
                          True
                          $else
                          False
                          $end
                          """;

        object data = new { A = a, B = b, C = c };

        string expected = expectedCondition ? "True" : "False";

        // Act
        string result = templater.RenderFromString(template, data);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [DataTestMethod]
    [DataRow(17, true)]
    [DataRow(18, false)]
    [DataRow(20, false)]
    public void RenderFromString_If_NegatedParentheses_ReturnValid(
        int age, bool expectedCondition)
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $if (!(Age >= 18))
                          Несовершеннолетний
                          $else
                          Совершеннолетний
                          $end
                          """;

        object user = new { Age = age };

        string expected = expectedCondition
            ? "Несовершеннолетний"
            : "Совершеннолетний";

        // Act
        string result = templater.RenderFromString(template, user);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();
        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [TestMethod]
    public void RenderFromString_If_NestedIf_ReturnValid()
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $if (IsAuthorized)
                          <h1>Добро пожаловать!</h1>
                          $if (IsAdmin)
                          <button>Панель администратора</button>
                          $else
                          <p>Обычный пользователь</p>
                          $end
                          $else
                          <h1>Доступ запрещён</h1>
                          $end
                          """;

        object user = new
        {
            IsAuthorized = true,
            IsAdmin = true
        };

        string expected = """
                          <h1>Добро пожаловать!</h1>
                          <button>Панель администратора</button>
                          """;

        // Act
        string result = templater.RenderFromString(template, user);
        expected = expected.Replace("\r\n", "\n").Trim();
        result = result.Replace("\r\n", "\n").Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [DataTestMethod]
    [DataRow(true, false, false, true, true)]
    [DataRow(false, false, true, true, false)]
    [DataRow(false, true, true, false, true)]
    [DataRow(false, false, false, false, false)]
    public void RenderFromString_If_NestedParentheses_ReturnValid(
        bool a, bool b, bool c, bool d, bool expectedCondition)
    {
        // Arrange
        var templater = new HtmlTemplateRenderer();

        string template = """
                          $if (((A || B) && (C || D)))
                          True
                          $else
                          False
                          $end
                          """;

        object data = new { A = a, B = b, C = c, D = d };

        string expected = expectedCondition ? "True" : "False";

        // Act
        string result = templater.RenderFromString(template, data);

        expected = expected.Trim();
        result = result.Trim();

        // Assert
        Assert.AreEqual(expected, result);
    }
}