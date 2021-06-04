
using Progra1Bot.Clases;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Progra1Bot
{
    class Program
    {
       
        private static TelegramBotClient Bot;

        public static async Task Main()
        {
            //await new clsEjemplo1().InicioEjemploTelegram();
           await new clsVersusBot().IniciarTelegram();
        }

    } // fin de la clase
  
}
































































// await new clsEjemplo2().IniciarTelegram();
// await new clsBotAlumnos().IniciarTelegram();

