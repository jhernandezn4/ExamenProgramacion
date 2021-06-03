using Progra1Bot.Clases.Alumnos;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text;


namespace Progra1Bot.Clases
{
    class clsBotAlumnos
    {
        private static TelegramBotClient Bot;

        public async Task IniciarTelegram()
        {
            Bot = new TelegramBotClient("PONER SU API");

            var me = await Bot.GetMeAsync();
            Console.Title = me.Username;

            Bot.OnMessage += BotCuandoRecibeMensajes;
            Bot.OnMessageEdited += BotCuandoRecibeMensajes;
            Bot.OnReceiveError += BotOnReceiveError;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"escuchando solicitudes del BOT @{me.Username}");



            Console.ReadLine();
            Bot.StopReceiving();
        }



        // cuando recibe mensajes
        private static async void BotCuandoRecibeMensajes(object sender, MessageEventArgs messageEventArgumentos)
        {
            var ObjetoMensajeTelegram = messageEventArgumentos;
            var mensajes = ObjetoMensajeTelegram.Message;

            string mensajeEntrante = mensajes.Text.ToLower();
    
            string id_manda_mensaje = mensajes.Chat.Id.ToString();
           

            string respuesta = "No te entiendo";
            if (mensajes == null || mensajes.Type != MessageType.Text)
                return;

            Console.WriteLine($"Recibiendo Mensaje del ID {id_manda_mensaje}.");
            Console.WriteLine($"Dice {mensajeEntrante}.");


            if (mensajeEntrante.Contains("hola"))
            {
                respuesta = emojis.mdEmoji.saludo+  "Hola me da mucho gusto de Saludarte!!!";
            }

            

            if (mensajeEntrante.Contains("@miumg.edu.gt"))
            {
                var alumno = new clsCapaNegocio().LocalizaAlumnoPorMail(mensajeEntrante, id_manda_mensaje);
                 if (alumno.nombre.StartsWith("no encontrado"))
                {
                    respuesta = "No se encuentra su correo";
                }
                else
                {
                    respuesta = "Hola " + alumno.nombre + " " + alumno.apellido + " qué gusto de verte por aquí \n"+" tu eres de la seccion "+alumno.seccion;
                }
            }

            if (mensajeEntrante.Contains("mis archivos"))
            {
                respuesta = new ClsManejoArchivos().ListaArchivos("C:\\tmp");
            }


                if (!String.IsNullOrEmpty(respuesta))//    
            {
                await Bot.SendTextMessageAsync(
                    chatId: ObjetoMensajeTelegram.Message.Chat,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown,
                    text: respuesta

            );
            }

        } // fin del metodo de recepcion de mensajes



        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("UPS!!! Recibo un error!!!: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }

    }//fin clase
}
