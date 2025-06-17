namespace SharpLox;

public class Token
{
    private TokenType type = new TokenType();
    private string lexeme;
    private Object? literal;
    private int line;
    
    public Token(TokenType type, string lexeme,  Object literal, int line)
    {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;
        this.line = line;
    }
    
    public new string ToString()
    {
        return type + " " +  lexeme + " " + literal;
    }
}