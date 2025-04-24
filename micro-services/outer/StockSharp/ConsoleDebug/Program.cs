using Ecng.Common;
using StockSharp.Algo;
using StockSharp.BusinessEntities;
using System.Net;
using System.Security;

namespace ConsoleDebug;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        StockSharp.Algo.Connector _connector = new();
        StockSharp.Quik.Lua.LuaFixMarketDataMessageAdapter luaFixMarketDataMessageAdapter = default!;
        StockSharp.Quik.Lua.LuaFixTransactionMessageAdapter luaFixTransactionMessageAdapter = default!;

        luaFixMarketDataMessageAdapter = new(_connector.TransactionIdGenerator)
        {
            Address = "localhost:5001".To<EndPoint>(),
            //Login = "quik",
            //Password = "quik".To<SecureString>(),
            IsDemo = true,
        };
        luaFixTransactionMessageAdapter = new(_connector.TransactionIdGenerator)
        {
            Address = "localhost:5001".To<EndPoint>(),
            //Login = "quik",
            //Password = "quik".To<SecureString>(),
            IsDemo = true,
        };
        _connector.Adapter.InnerAdapters.Add(luaFixMarketDataMessageAdapter);
        _connector.Adapter.InnerAdapters.Add(luaFixTransactionMessageAdapter);

        _connector.Connected += _connector_Connected;
        _connector.ConnectedEx += _connector_ConnectedEx;
        _connector.ConnectionError += _connector_ConnectionError;
        _connector.ConnectionErrorEx += _connector_ConnectionErrorEx;
        _connector.ConnectionLost += _connector_ConnectionLost;

        _connector.PortfolioReceived += _connector_PortfolioReceived;

        _connector.SecurityReceived += _connector_SecurityReceived;

        _connector.Connect();

        Console.WriteLine(_connector.ConnectionState);
        Console.ReadLine();
        Console.ReadLine();
        static void _connector_ConnectionLost(StockSharp.Messages.IMessageAdapter obj)
        {
            Console.WriteLine($"connector ConnectionLost: {obj}");
        }

        static void _connector_ConnectionErrorEx(StockSharp.Messages.IMessageAdapter arg1, Exception arg2)
        {
            Console.WriteLine("connector ConnectionErrorEx");
        }

        static void _connector_ConnectionError(Exception obj)
        {
            Console.WriteLine("connector ConnectionError");
        }

        static void _connector_ConnectedEx(StockSharp.Messages.IMessageAdapter obj)
        {
            Console.WriteLine($"connector ConnectedEx: {obj}");
        }

        void _connector_PortfolioReceived(Subscription arg1, Portfolio arg2)
        {
            //Portfolio[] myPortfolios = [.. _connector.Portfolios];
            Console.WriteLine($"connector PortfolioReceived: {arg1} {arg2}");
        }

        static void _connector_Connected()
        {
            Console.WriteLine("connector Connected");
        }
    }
    static void _connector_SecurityReceived(Subscription arg1, Security arg2)
    {
        Console.WriteLine($"SecurityReceived ({arg1}) - {arg2}");
    }

}
