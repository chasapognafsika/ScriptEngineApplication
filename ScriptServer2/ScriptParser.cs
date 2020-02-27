using System;
using System.Collections.Generic;
using Irony.Parsing;
using NLog;

namespace ScriptServer
{
    public class ScriptParser
    {
        public List<ScriptEngineParams> _parms = new List<ScriptEngineParams>();
        private static readonly ILogger ErrorLogger = LogManager.GetLogger("fileErrorLogger");

        public ScriptParser(string script)
        {
            ScriptGrammar ex = new ScriptGrammar();
            Parser parser = new Parser(ex);
            ParseTree parseTree = parser.Parse(script);
            if (!parseTree.HasErrors())
            {
                ParseTree(parseTree.Root, 0);
            }
            else
            {
                string errors = "Error in parsing";
                foreach (var s in parseTree.ParserMessages)
                    errors += "\n" + s;
                ErrorLogger.Info(errors);
                throw new Exception(errors);
            }
        }

        public void ParseTree(ParseTreeNode node, int level)
        {
            if (level == 1)
            {
                switch (node.ChildNodes[0].Token.Text)
                {
                    case "select-window":
                        _parms.Add(new SelectWindowParams { window = node.ChildNodes[1].Token.Text });
                        break;
                    case "mouse-move":
                        _parms.Add(new SetCursorParams
                        {
                            x = Int32.Parse(node.ChildNodes[1].Token.Text),
                            y = Int32.Parse(node.ChildNodes[3].Token.Text)
                        });
                        break;
                    case "mouse-click":
                        _parms.Add(new MouseClickParams()
                        {
                            click = node.ChildNodes[1].Token.Text == "single-click" ?
                                        MouseClickParams.clicktype.single_click : MouseClickParams.clicktype.double_click
                        });
                        break;
                    case "send-keys":
                        _parms.Add(new SendKeysParams
                        {
                            str = node.ChildNodes[1].Token.Text
                        });
                        break;
                }
            }
            foreach (ParseTreeNode child in node.ChildNodes)
                ParseTree(child, level + 1);
        }
    }
}
