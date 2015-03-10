using System;
using Akka.Actor;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for reading FROM the console. 
    /// Also responsible for calling <see cref="ActorSystem.Shutdown"/>.
    /// </summary>
    class ConsoleReaderActor : UntypedActor
    {
        public const string ExitCommand = "exit";
        public const string StartCommand = "start";

        protected override void OnReceive(object message)
        {
            if (message.Equals(StartCommand))
                DoPrintInstruction();

            GetAndValidateInput();
        }

        private void GetAndValidateInput()
        {
            var message = Console.ReadLine();
            if (!string.IsNullOrEmpty(message) && message.ToLowerInvariant().Equals(ExitCommand))
            {
                Context.System.Shutdown();
                return;
            }

            Context.ActorSelection("/user/validationActor").Tell(message);
        }

        private void DoPrintInstruction()
        {
            Console.WriteLine("Please provide the URI of a log file on disk.\n");
        }
    }
}