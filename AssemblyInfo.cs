using System.Collections.Generic;
using System.Runtime.CompilerServices;

// MSTestからinternalクラスのアクセスを受け入れるための設定
[assembly: InternalsVisibleTo("OOP_RPG.xUnitTests")]

// internalクラスにモックを注入するための設定
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]