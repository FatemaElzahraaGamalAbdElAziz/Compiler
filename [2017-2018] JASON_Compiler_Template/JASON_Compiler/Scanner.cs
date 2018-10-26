using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Token_Class
{
    Else, If, ElseIf, Read, Then, Until, Write, EqualOp, LessThanOp, 
    GreaterThanOp, NotEqualOp, PlusOp, MinusOp, MultiplyOp, DivideOp,
    Idenifier, Constant, String, Float, Integer ,And ,Or, Endl ,Error
}

namespace JASON_Compiler
{
    

    public class Token
    {
       public string lex;
       public Token_Class token_type;
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        public List<Token> Errors = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("elseif", Token_Class.ElseIf);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("int", Token_Class.Integer);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("endl", Token_Class.Endl);


           
            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<",Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("<>", Token_Class.NotEqualOp);
            Operators.Add("&&", Token_Class.And);
            Operators.Add("||", Token_Class.Or);
            


        }

    public void StartScanning(string SourceCode)
        {
            for(int i=0; i<SourceCode.Length;i++)
            {
                char CurrentChar = SourceCode[i];
                string CurrentLexeme ="";

               

                if (CurrentChar == ' ' || CurrentChar == '\r' || CurrentChar == '\n')
                    continue;

            
                if ((CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z') ) 
                {
                   
                    while (  (CurrentChar >= 'A' && CurrentChar <= 'Z') || (CurrentChar >= 'a' && CurrentChar <= 'z') || (CurrentChar >= '0' && CurrentChar <= '9'))
                   {
                       CurrentLexeme += CurrentChar;
                       i++;
                       if (i >= SourceCode.Length)
                       {
                  
                           break;
                       }
                       CurrentChar = SourceCode[i];
                       
                      
                   }
                   i--;
                   FindTokenClass(CurrentLexeme);
                   CurrentLexeme = "";
                   

                   
                }

                else if (CurrentChar >= '0' && CurrentChar <= '9')
                {
                    CurrentLexeme = "";
                    bool point = false;

                    while (true)
                    {

                        if ( (CurrentChar >= '0' && CurrentChar <= '9' ) || CurrentChar =='.')
                        {
                            CurrentLexeme += CurrentChar;
                            i++;
                            if (CurrentChar == '.')
                            {
                                if (!point)
                                {
                                    point = true;
                                }
                                else
                                {
                                    Console.WriteLine("daa error maynfa3sh aktar mn 1 point");
                                    Token error=new Token();
                                    error.lex= CurrentLexeme;
                                    error.token_type=Token_Class.Error;
                                    Errors.Add(error);
                                    break ;
                                }
                            }
                             if (i < SourceCode.Length) CurrentChar = SourceCode[i];
                            else break;
                        }
                        else
                        {
                            //momkn yb2a 7aga ghalat w momkn ykhlas l raqam " "
                            if (CurrentChar == ' ')
                            {
                                FindTokenClass(CurrentLexeme);
                                CurrentLexeme = "";

                            }
                            else
                            {
                                Console.WriteLine("ERRROR!!!!!!!!!!");
                                Token error = new Token();
                                error.lex = CurrentLexeme;
                                error.token_type = Token_Class.Error;
                                Errors.Add(error);
                        
                            }
                            break;
                        }
                    }

                }
                    //comments
                else if (CurrentChar == '/' && i+1 != SourceCode.Length - 1 && SourceCode[i+1]== '*')
                {
              
                    i+=2;
                    while (true)
                    {
                        if (i+1 != SourceCode.Length - 1 && SourceCode[i] == '*' && SourceCode[i + 1] == '/')
                        {
                            i++;
                            CurrentLexeme = "";
                            break;
                        }

                        i++;
                    }
                    
                }
                else if (CurrentChar == '+' || CurrentChar == '-' || CurrentChar == '*' || CurrentChar == '/' || CurrentChar == '=' || CurrentChar == '<' || CurrentChar =='>')
                {
                    CurrentLexeme = "";
                    CurrentLexeme += CurrentChar;
                    if (CurrentChar == '<' && i + 1 < SourceCode.Length && SourceCode[i + 1] == '>')
                    {
                        i++;
                        CurrentLexeme += SourceCode[i];
                    }
                    FindTokenClass(CurrentLexeme);
                    CurrentLexeme = "";

                }
                    //   && ||
                else if ((CurrentChar == '&' || CurrentChar == '|') && (i + 1 < SourceCode.Length && SourceCode[i + 1] == CurrentChar))
                {
                    
                        i++;
                        CurrentLexeme = "";
                        CurrentLexeme += CurrentChar;
                        CurrentLexeme += CurrentChar;
                        FindTokenClass(CurrentLexeme);
                        CurrentLexeme = "";

                    
                }
                else if(CurrentChar == '"'){
                    CurrentLexeme += CurrentChar;
                    i++;
                    CurrentChar = SourceCode[i];
                    while (CurrentChar != '"')
                    {
                        CurrentLexeme += CurrentChar;
                        i++;
                        CurrentChar = SourceCode[i];
                    }
                    CurrentLexeme += CurrentChar;
                    FindTokenClass(CurrentLexeme);
                    CurrentLexeme = "";
                }
                else
                {
                    Console.WriteLine("ERROR!!!!!!!!!!!!!!!!!");
                    Token error = new Token();
                    error.lex = CurrentLexeme;
                    error.token_type = Token_Class.Error;
                    Errors.Add(error);
                    
                }
            }
            
            JASON_Compiler.TokenStream = Tokens;
        }
        void FindTokenClass(string Lex)
        {
            Token Tok = new Token();
            Tok.lex = Lex;
            

            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
            }
            
            else   if ((Lex[0] >= 'A' && Lex[0] <= 'Z') || (Lex[0] >= 'a' && Lex[0] <= 'z'))
            {
                Tok.token_type = Token_Class.Idenifier;
            }

            else if (Lex[0] >= '0' && Lex[0] <= 9)
            {
                Tok.token_type = Token_Class.Constant;
            }

            else if(Lex[0] == '+' || Lex[0] == '-' || Lex[0] == '*' || Lex[0] == '/' || Lex[0] =='<' || Lex[0] =='>' || Lex[0]=='=')
            {
                Tok.token_type = Operators[Lex];
            }
            else if (Lex[0] == '"')
            {
                Tok.token_type = Token_Class.String;
            }
               
            Tokens.Add(Tok);

        }

        bool isIdentifier(string lex)
        {
            bool isValid = true;


            return isValid;
        }
        bool isConstant(string lex)
        {
            bool isValid = true;

            return isValid;
        }
    }
}
