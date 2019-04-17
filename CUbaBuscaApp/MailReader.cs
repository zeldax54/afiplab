using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Pop3;
using MailKit.Security;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace CUbaBuscaApp
{
    public static class MailReader
    {


        //public static List<Datos> ReadMail()
        //{
        //    try
        //    {
        //        Helper.DoLog("Leer bandeja de entrada");
        //        List<Datos> final = new List<Datos>();
        //        //  DownloadNewMessages(null);
        //        using (var client = new Pop3Client())
        //        {

        //            var Server = "pop.gmail.com";
        //            var Port = "995";
        //            var UseSsl = true;
        //            var credentials = new NetworkCredential("cubabusca@gmail.com", "hectorluis001");
        //            var cancel = new CancellationTokenSource();
        //            var uri = new Uri(string.Format("pop{0}://{1}:{2}", (UseSsl ? "s" : ""), Server, Port));
        //            //Connect to email server
        //            client.Connect(uri, cancel.Token);
        //            // client.Connect("pop.gmail.com", 995, SecureSocketOptions.SslOnConnect,cancel.Token);
        //            client.AuthenticationMechanisms.Remove("XOAUTH2");
        //            client.Authenticate(credentials, cancel.Token);

        //            for (int i = 0; i < client.Count; i++)
        //            {
        //                var message = client.GetMessage(i);
        //                var asunto = message.Subject;
        //                var nombre = message.From[0].Name;
        //                var correo = message.From.Mailboxes.FirstOrDefault().Address;
        //                Datos d = new Datos
        //                {
        //                    busqueda = asunto,
        //                    correo = correo,
        //                    nombre = nombre,
        //                    horaproceso = DateTime.Now

        //                };
        //                final.Add(d);
        //            }

        //            client.Disconnect(true);
        //        }
        //        return final;
        //    }
        //    catch (Exception e)
        //    {
        //        Helper.DoLog(e.Message);
        //    }
        //    Helper.DoLog("Sin Mensajes nuevos");
        //    return null;

        //}


        //public static void DownloadNewMessages(HashSet<string> previouslyDownloadedUids)
        //{
        //    using (var client = new Pop3Client())
        //    {
        //        client.Connect("pop.gmail.com", 995, SecureSocketOptions.SslOnConnect);

        //        client.Authenticate("username", "password");

        //        if (!client.Capabilities.HasFlag(Pop3Capabilities.UIDL))
        //            throw new Exception("The POP3 server does not support UIDs!");

        //        var uids = client.GetMessageUids();

        //        for (int i = 0; i < client.Count; i++)
        //        {
        //            // check that we haven't already downloaded this message
        //            // in a previous session
        //            if (previouslyDownloadedUids.Contains(uids[i]))
        //                continue;

        //            var message = client.GetMessage(i);

        //            // write the message to a file
        //            message.WriteTo(string.Format("{0}.msg", uids[i]));

        //            // add the message uid to our list of downloaded uids
        //            previouslyDownloadedUids.Add(uids[i]);
        //        }

        //        client.Disconnect(true);
        //    }
        //}


        //public static void SendMail(List<Datos> datos,string cuerpo)
        //{
        //    try
        //    {
        //        Helper.DoLog("Enviando Respuestas");
        //        var message = new MimeMessage();
        //        message.From.Add(new MailboxAddress("CubaBusca", "cubabusca@gmail.com"));
        //        foreach (var d in datos)
        //            message.To.Add(new MailboxAddress(d.nombre, d.correo));
        //        message.Subject = "Resultados de su busqueda";
        //        message.Body = new TextPart("plain")
        //        {
        //            Text = cuerpo
        //        };

        //        using (var client = new SmtpClient())
        //        {
        //            client.Connect("smtp.gmail.com", 587);

        //            // Note: since we don't have an OAuth2 token, disable
        //            // the XOAUTH2 authentication mechanism.
        //            client.AuthenticationMechanisms.Remove("XOAUTH2");
        //            var credentials = new NetworkCredential("cubabusca@gmail.com", "hectorluis001");
        //            client.AuthenticationMechanisms.Remove("XOAUTH2");
        //            var cancel = new CancellationTokenSource();
        //            client.Authenticate(credentials, cancel.Token);
        //            client.Send(message);
        //            client.Disconnect(true);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Helper.DoLog(e.Message);
        //    }
           
        //}


    }
}
