using System;
using System.Data;
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


namespace Progra1Bot.Clases
{

   public  class clsVersusBot
    {
        private static TelegramBotClient Bot;

        private clsConexion conexion;
        public bool preguntando { get; set; }
        public  async Task IniciarTelegram()
        {
            Bot = new TelegramBotClient("EL API TOKEN AQUI");
           
            var me = await Bot.GetMeAsync();
            Console.Title = me.Username;

            Bot.OnMessage += BotCuandoRecibeMensajes;
            Bot.OnMessageEdited += BotCuandoRecibeMensajes;
            Bot.OnReceiveError += BotOnReceiveError;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"escuchando solicitudes del BOT @{me.Username} ");

           

            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotCuandoRecibeMensajes(object sender, MessageEventArgs messageEventArgumentos)
        {
            var ObjetoMensajeTelegram = messageEventArgumentos;
            var mensajes = ObjetoMensajeTelegram.Message;

            string mensajeEntrante = mensajes.Text;


            
            Console.WriteLine($"Recibiendo Mensaje del chat {ObjetoMensajeTelegram.Message.Chat.Id} {ObjetoMensajeTelegram.Message.Chat.Username}.");
            Console.WriteLine($"Dice {ObjetoMensajeTelegram.Message.Text}.");
            
            var message = ObjetoMensajeTelegram.Message;
            if (message == null || message.Type != MessageType.Text)
                return;

            switch (message.Text.Split(' ').First())
            {
                case "/preguntame":
                    await Preguntar(message);
                    break;
                case "/miposicion":
                    await MiLugar(message);
                    break;
                
                case "/podio":
                    await Top10(message);
                    break;
                case "/restablecer":
                    await Restablecer(message);
                    break;
                default:
                    await AnalizarRespuesta(message);
                    break;
            }

        } // fin del metodo de recepcion de mensajes
        static async Task Preguntar(Message message)
        {
            clsConexion conexion = new clsConexion(message.Chat.Id);
            DataTable Posicion = conexion.Posicion();
            
            if (Posicion.Rows.Count == 0)
            {
                Posicion = conexion.Registrar(message.Chat.Id, message.Chat.Username);
            }
            string pregunta = Posicion.Rows[0]["pregunta_actual"].ToString();
            int NuevaPregunta = Convert.ToInt32(pregunta);
            if (NuevaPregunta == 0)
            {
                NuevaPregunta = 1;
            }
            else
            {
                NuevaPregunta = NuevaPregunta + 1;
            }
            if (NuevaPregunta > 25)
            {
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Preguntas Finalizadas, Inicamos de nuevo :)"
                );
                NuevaPregunta = 1;
            }
            DataTable Preguntona = conexion.ObtenerPregunta(NuevaPregunta);

            Random rnd = new Random();
            string izquierda = Preguntona.Rows[0]["correcta"].ToString();
            string derecha = Preguntona.Rows[0]["incorrecta"].ToString();
            if (rnd.Next(2) == 0)
            {
                 izquierda = Preguntona.Rows[0]["correcta"].ToString();
                 derecha = Preguntona.Rows[0]["incorrecta"].ToString();
            }
            else
            {
                 izquierda = Preguntona.Rows[0]["incorrecta"].ToString();
                 derecha = Preguntona.Rows[0]["correcta"].ToString();
            }
            


            var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                   new KeyboardButton[][]
                   {
                        new KeyboardButton[] { izquierda, derecha },
                   },
                   resizeKeyboard: true
               );
            await Bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: Preguntona.Rows[0]["pregunta"].ToString(),
                replyMarkup: replyKeyboardMarkup
            );
        }
        static async Task MiLugar(Message message)
        {
            clsConexion conexion = new clsConexion(message.Chat.Id);
            DataTable Posicion = conexion.MostrarPosiciones();
            if (Posicion.Rows.Count == 0)
            {
                Posicion = conexion.Registrar(message.Chat.Id, message.Chat.Username);
            }
            int Index = 1;
            string lugar = "No estas registrado, inicia el juego para incluirte";
            foreach (DataRow fila in Posicion.Rows)
            {
                if (fila["idchat"].ToString() == message.Chat.Id.ToString())
                {
                    lugar = "#" + Index + " " + fila["usuario"].ToString() + " con " + fila["punteo"].ToString() + " puntos";
                }
                Index++;
            }
            await Bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: lugar
            );
        }
        static async Task Top10(Message message)
        {
            clsConexion conexion = new clsConexion(message.Chat.Id);
            DataTable Posicion = conexion.MostrarPosiciones();

            if (Posicion.Rows.Count == 0)
            {
                Posicion = conexion.Registrar(message.Chat.Id, message.Chat.Username);
            }
            int Index = 1;
            string lugar = "";
            foreach (DataRow fila in Posicion.Rows)
            {

                if (Index<11)
                {
                    lugar += "#" + Index + " " + fila["usuario"].ToString() + " con " + fila["punteo"].ToString() + " puntos\n";
                }
                Index++;
            }
            await Bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: lugar
            );
        }
        static async Task Restablecer(Message message)
        {
            clsConexion conexion = new clsConexion(message.Chat.Id);
            DataTable Posicion = conexion.MostrarPosiciones();

            conexion.RestablecerPuntaje();
            await Bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: "Tu punteo e intentos fueron restablecidos"
            );
            await uso(message);
        }
        static async Task AnalizarRespuesta(Message message)
        {
            clsConexion conexion = new clsConexion(message.Chat.Id);
            DataTable posicion = conexion.Posicion();

            if (posicion.Rows.Count == 0)
            {
                posicion = conexion.Registrar(message.Chat.Id, message.Chat.Username);
            }
            int pregunta = Convert.ToInt32(posicion.Rows[0]["pregunta_actual"].ToString());
            int resuelto = Convert.ToInt32(posicion.Rows[0]["resuelto"].ToString());

            if (resuelto == 0)
            {
                DataTable Preguntona = conexion.ObtenerPregunta(pregunta);

                if(Preguntona.Rows[0]["correcta"].ToString()==message.Text.ToString())
                {
                    conexion.Responder(true);
                    await Bot.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Correcto"
                    );
                    await Preguntar(message);
                }
                else if (Preguntona.Rows[0]["incorrecta"].ToString() == message.Text.ToString())
                {
                    conexion.Responder(false);
                    await Bot.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Incorrecto"
                    );
                    await Preguntar(message);
                }
                else
                {
                    
                    await uso(message);
                }

            }
            else
            {
                await uso(message);
            }
            


          
        }
        
        static async Task uso(Message message)
        {
            string usage = $"Hola {message.Chat.Username} selecciona una opción :\n" +
                                    "/preguntame   - Responder pregunta\n" +
                                    "/miposicion - Ver mi posicion actual\n" +
                                    "/podio    - Ver el top 5 de posiciones\n"+
                                    "/restablecer    - Restablece tu puntaje e intentos\n";
            await Bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                replyMarkup: new ReplyKeyboardRemove()
            );
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("UPS!!! Recibo un error!!!: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }


    }
}
