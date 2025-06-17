namespace SharpLox;

public class Scanner
{
    private string source;
    private List<Token> tokens = new List<Token>();
    private int start = 0;
    private int current = 0;
    private int line = 1;

    Scanner (String source)
    {
        this.source = source;
    }

    List<Token> Tokenize()
    {
        while (!IsAtEnd())
        {
            start = current;
            ScanToken();
        }

        Token NewToken = new Token(TokenType.EOF, "", null, line);
        tokens.Add(NewToken);
        return tokens;
    }

    private bool IsAtEnd()
    {
        return current >= source.Length;
    }

    public void ScanToken()
    {
        char c = Advance();

        switch (c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break; 
            case '!': AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
            case '=': AddToken(Match('=') ?  TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
            case '<': AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
            case '>': AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
            case '/':
                if (Match('/'))
                {
                    while (Peek() != '\n' && !IsAtEnd())
                    {
                        Advance();
                    }
                }
                else
                {
                    AddToken(TokenType.SLASH);
                }

                break;
            
            case ' ':
            case '\r':
            case '\t':
                break;
            
            case '\n':
                line++;
                break;
            
            case '"': String(); break;
                
                
            
            
            default:
                if (IsDigit(c))
                {
                    Number();
                }
                else
                {
                    Lox.Error(line, "Unexpected character: " + c);
                }
                
                break;
        }
    }

    private void Number()
    {
        while (IsDigit(Peek())) Advance();
        
        // Look for a fraction
        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            Advance();
            
            while (IsDigit(Peek())) Advance();
            
            AddToken(TokenType.NUMBER, Double.Parse(source.Substring(start, current)));
        }
        
        
    }

    private void String()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n' && !IsAtEnd())
            {
                line++;
            }
            Advance();
        }

        if (IsAtEnd())
        {
            Lox.Error(line, "Unterminated string.");
            return;
        }
        
        Advance();
        
        string value = source.Substring(start, current - start);
        AddToken(TokenType.STRING, value);
    }

    private bool Match(char expected)
    {
        if (IsAtEnd()) return false;
        if (source[current] != expected) return false;
        
        current++;
        return true;
    }

    // Look ahead without consuming the current character
    private char Peek()
    {
        if (IsAtEnd()) return '\0';
        return source[current];
    }

    private char PeekNext()
    {
        if (current + 1 >= source.Length) return '\0';
        
        return source[current + 1];
    }

    private bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private char Advance()
    {
        return source[current++];
    }

    private void AddToken(TokenType type)
    {
        AddToken(type, null);
    }

    private void AddToken(TokenType type, Object literal)
    {
        string text = source.Substring(start, current);
        tokens.Add(new Token(type, text, literal, line));
    }
}