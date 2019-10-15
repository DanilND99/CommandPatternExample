using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandPattern
{
    class Program{
        static void Main(string[] args){
            var modify = new ModifyWallet();
            var wallet = new Wallet(1000);
            Execute(wallet, modify, new WalletCommand(wallet, WalletAction.Add, 350));
            Execute(wallet, modify, new WalletCommand(wallet, WalletAction.Substract, 70));
            Execute(wallet, modify, new WalletCommand(wallet, WalletAction.Substract, 200));
            Execute(wallet, modify, new WalletCommand(wallet, WalletAction.Add, 50));
            Console.WriteLine(wallet);
            modify.UndoActions();
            Console.WriteLine(wallet);
        }
        private static void Execute(Wallet wallet, ModifyWallet modify, ICommand walletCommand){
            modify.SetCommand(walletCommand);
            modify.Invoke();
        }
    }

    public class Wallet{
        public int  Quantity{ get; set; }
        public Wallet(int quantity){
            Quantity = quantity;
        }
        public void Add(int number){
            Quantity += number;
            Console.WriteLine("You added ${0} to your wallet. Your money is now ${1}.", number, Quantity);
        }
        public void Substract(int number){
            Quantity -= number;
            Console.WriteLine("You Substracted ${0} from your wallet. Your money is now ${1}.", number, Quantity);
        }
        public override string ToString() => $"You have ${Quantity} on your wallet.";
    }

    public enum WalletAction{
        Add,
        Substract
    }

    public interface ICommand{
        void Execute();
        void Undo();
    }

    public class WalletCommand : ICommand{
        private readonly Wallet _wallet;
        private readonly WalletAction _action;
        private readonly int _number;
        public WalletCommand(Wallet wallet, WalletAction action, int number){
            _wallet = wallet;
            _action = action;
            _number = number;
        }
        public void Execute(){
            if(_action == WalletAction.Add){
                _wallet.Add(_number);
            }else{
                _wallet.Substract(_number);
            }
        }
        public void Undo(){
            if (_action == WalletAction.Add){
                _wallet.Substract(_number);
            }else{
                _wallet.Add(_number);
            }
        }
    }

    public class ModifyWallet{
        private readonly List<ICommand> _commands;
        private ICommand _command;
        public ModifyWallet(){
            _commands = new List<ICommand>();
        }
        public void SetCommand(ICommand command) => _command = command;
        public void Invoke(){
            _commands.Add(_command);
            _command.Execute();
        }
        public void UndoActions(){
            foreach (var command in Enumerable.Reverse(_commands)){
                command.Undo();
            }
        }
    }
}//Original Code from: https://code-maze.com/command/