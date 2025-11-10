
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Markdown.Tests;
[TestFixture]
public class SymbolRulesTests
{
    private SymbolRules rules;
    private CharReader reader;

    [SetUp]
    public void Setup()
    {
        rules = new SymbolRules();
    }

    [TestCase("# Привет", 0, ExpectedResult = true)]
    [TestCase("Текст\n# Привет", 6, ExpectedResult = true)]
    [TestCase("Текст # Привет", 6, ExpectedResult = false)]
    public bool IsHeaderTest(string input, int position)
    {
        reader = new CharReader(input);
        reader.MovePositions(position);
        return rules.IsHeader(reader.GetSymbol(), reader);
    }

    [TestCase("__bold__", 0, ExpectedResult = true)]
    [TestCase("_italic_", 0, ExpectedResult = false)]
    public bool IsBoldTest(string input, int pos)
    {
        reader = new CharReader(input);
        reader.MovePositions(pos);
        return rules.IsBold(reader.GetSymbol(), reader);
    }

    [Test]
    public void InItalicTest()
    {
        var stack = new Stack<Token>();
        stack.Push(new Token(TokenType.Italic));
        Assert.IsTrue(rules.InItalic(stack));
    }

    [Test]
    public void InBoldTest()
    {
        var stack = new Stack<Token>();
        stack.Push(new Token(TokenType.Bold));
        Assert.IsTrue(rules.InBold(stack));
    }

    [TestCase("a _b", 1, ExpectedResult = true)]
    [TestCase("a_b", 1, ExpectedResult = false)]
    public bool NextSpaceTest(string input, int pos)
    {
        reader = new CharReader(input);
        reader.MovePositions(pos);
        return rules.NextSpace(reader);
    }

    [Test]
    public void EmptyLineTest()
    {
        reader = new CharReader("____");
        var sb = new StringBuilder();
        var result = rules.EmptyLine(reader, sb);

        Assert.IsTrue(result);
        Assert.AreEqual("\\_\\_\\_\\_", sb.ToString());
    }

    [TestCase("_a_", 0, ExpectedResult = true)]
    [TestCase("a_12_", 1, ExpectedResult = false)]
    public bool IsItalicTest(string input, int pos)
    {
        reader = new CharReader(input);
        reader.MovePositions(pos);
        return rules.IsItalic(reader.CheckNextPositions(0), reader);
    }
}