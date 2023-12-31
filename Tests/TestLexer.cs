using Xunit;
using Interpreter;

namespace Tests;

public class TestLexer
{
    [Fact]
    public void TestInteger() {
        Lexer l = new Lexer("le 5;");
        l.GetNextToken();
        Assert.Equal(l.GetNextToken(), new Token(Tokens.INTEGER, "5"));
    }

    [Fact]
    public void TestString() {
        Lexer l = new Lexer("blob doko \"lorem\" noger;");
        l.GetNextToken();
        l.GetNextToken();
        Assert.Equal(l.GetNextToken(), new Token(Tokens.STRING, "lorem"));
    }

    [Fact]
    public void TestBadString() {
        Lexer l = new Lexer("blob doko \"lorem noger;");
        l.GetNextToken();
        l.GetNextToken();
        Assert.Throws<LexingError>(l.GetNextToken);
    }
    [Fact]
    public void TestBadID()
    {
        Lexer l = new Lexer("1a");
        Assert.Throws<LexingError>(l.GetNextToken);
    }

    [Fact]
    public void TestFloat() {
        Lexer l = new Lexer("2.71;");
        Assert.Equal(l.GetNextToken(), new Token(Tokens.FLOAT, "2.71"));
    }

    [Fact]
    public void TestAssign() {
        Lexer l = new Lexer("let a = \"hello world\"");
        Assert.Equal(l.GetNextToken(), new Token(Tokens.LET, "let"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "a"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ASSIGN, "="));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.STRING, "hello world"));
    }

    [Fact]
    public void TestFunction() {
        Lexer l = new Lexer("a(n) = 5 ");
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "a"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.LPAREN, "("));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "n"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.RPAREN, ")"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ASSIGN, "="));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.INTEGER, "5"));
    }

    [Fact]
    public void TestConditional() {
        Lexer l = new Lexer("if 0 then \"blob\" else \"doko\"");
        Assert.Equal(l.GetNextToken(), new Token(Tokens.IF, "if"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.INTEGER, "0"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.THEN, "then"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.STRING, "blob"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ELSE, "else"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.STRING, "doko"));
    }

    [Fact]
    public void TestNewline() {
        Lexer l = new Lexer("hello\nworld");
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "hello"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "world"));
    }
    [Fact]
    public void TestConstant()
    {
        Lexer l = new Lexer("m = measure(p1,p2)");
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "m"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ASSIGN, "="));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "measure"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.LPAREN, "("));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "p1"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.COMMA, ","));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "p2"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.RPAREN, ")"));
    }
    [Fact]
    public void TestSequence()
    {
        Lexer l = new Lexer("a,b,_ = { p1 }");
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "a"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.COMMA, ","));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "b"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.COMMA, ","));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "_"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ASSIGN, "="));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.SEQUENCE_START, "{"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "p1"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.SEQUENCE_END, "}"));
    }
    [Fact]
    public void TestMultiLine()
    {
        Lexer l = new Lexer("color blue;\ndraw line(p1, p2);\nrestore;");
        Assert.Equal(l.GetNextToken(), new Token(Tokens.COLOR, "color"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "blue"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.END, ";"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.DRAW, "draw"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "line"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.LPAREN, "("));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "p1"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.COMMA, ","));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.ID, "p2"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.RPAREN, ")"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.END, ";"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.RESTORE, "restore"));
        Assert.Equal(l.GetNextToken(), new Token(Tokens.END, ";"));
    }

    [Fact]
    public void TestHL()
    {
        Lexer l = new Lexer(">= 1 <= 2");
        Assert.Equal(l.GetNextToken(), new Token(Tokens.HIGHEREQUAL, ">="));
        l.GetNextToken();
        Assert.Equal(l.GetNextToken(), new Token(Tokens.LOWEREQUAL, "<="));
    }

}
