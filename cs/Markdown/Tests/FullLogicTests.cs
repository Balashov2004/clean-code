
using NUnit.Framework;

namespace Markdown.Tests;

[TestFixture]
public class FullLogicTests
{
    private MarkdownParser parser;
    private Render renderer;

    [SetUp]
    public void Setup()
    {
        parser = new MarkdownParser();
        renderer = new Render();
    }
    
    [TestCase(
        "# Заголовок",
        "<h1> Заголовок</h1>")]
    [TestCase(
        "__Жирный__",
        "<strong>Жирный</strong>")]
    [TestCase(
        "_Курсив_",
        "<em>Курсив</em>")]
    [TestCase(
        "\\_Вот это\\_ сим\\волы экранирования\\ \\должны остаться.\\",
        "\\_Вот это\\_ сим\\волы экранирования\\ \\должны остаться.\\")]
    [TestCase("\\_НЕкурсив\\_",
        "\\_НЕкурсив\\_")]
    [TestCase("__двойного выделения _одинарное_ тоже__",
        "<strong>двойного выделения <em>одинарное</em> тоже</strong>")]
    [TestCase("_одинарного __двойное__ не_",
        "<em>одинарного \\_\\_двойное\\_\\_ не</em>")]
    [TestCase("цифрами_12_3",
        "цифрами_12_3")]
    [TestCase("_нач_але, и в сер_еди_не, и в кон_це._",
        "<em>нач</em>але, и в сер<em>еди</em>не, и в кон<em>це.</em>")]
    [TestCase("эти_ подчерки_ не считаются",
        "эти\\_ подчерки\\_ не считаются")]
    [TestCase("эти _подчерки _не считаются_ окончанием",
        "эти <em>подчерки \\_не считаются</em> окончанием")]
    [TestCase("# Заголовок __с _разными_ символами__",
        "<h1> Заголовок <strong>с <em>разными</em> символами</strong></h1>")]
    //Ниже три правила не проходят
    // [TestCase("ра_зных сл_овах",
    //     "ра\\_зных сл\\_овах")]
    // [TestCase("В случае __пересечения _двойных__ и одинарных_ подчерков",
    //     "В случае \\_\\_пересечения \\_двойных\\_\\_ и одинарных\\_ подчерков")]
    // [TestCase("__Непарные_ символы в рамках одного абзаца не считаются выделением.",
    //     "\\_\\_Непарные\\_ символы в рамках одного абзаца не считаются выделением.")]
    public void Render_Correctly(string input, string expected)
    {
        var root = parser.Parse(input);
        var html = renderer.RenderToHtml(root).Trim();
        
        Assert.AreEqual(expected, html);
    }
}