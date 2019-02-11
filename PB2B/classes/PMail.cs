using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace PB2B.classes
{
    public class PMail
    {
        public static string MailGonderSiparis(string mesaj, string Tutar, List<string> Kime, List<string> LUrunKodu, string Subj)
        {

            string GFrom = "rafet@mentalsoft.net";
            string GSubject = Subj;
            string smtpserver = "mail.mentalsoft.net";
            try
            {
                //mail gönderilcek hesabı bul
                //mail gönder
                MailMessage ePosta = new MailMessage();
                ePosta.From = new MailAddress(GFrom);
                //dpt yoneticilerini bul ccye ekle
                foreach (var item in Kime)
                {
                    ePosta.To.Add(item);
                }
                ePosta.CC.Add("rafet@mentalsoft.net");
                StringBuilder sb = new StringBuilder();
                sb.Append(" <div class=WordSection1> <p class=MsoNormal> <span style='font-size:11.0pt;font-family:' Calibri','sans-serif';color:#1F497D'><o:p>&nbsp;</o:p></span> </p><p class=MsoNormal> <span style='font-size:11.0pt;font-family:' Calibri','sans-serif';color:#1F497D'><o:p>&nbsp;</o:p></span> </p><p class=MsoNormal> <o:p>&nbsp;</o:p> </p> <div style='border:solid #E1DFDF 1.0pt;border-top:solid #00ACFE 2.25pt;padding:0cm 0cm 0cm 0cm'><div style='border:none;border-bottom:solid #E1DFDF 1.0pt;padding:0cm 0cm 0cm 0cm;margin-left:1.0pt;margin-top:7.5pt'><div style='margin-top:18.75pt'><p class=MsoNormal align=center style='text-align:center;background:white'><span style='font-size:22.5pt'><img src='http://favio.com.tr/img/logo.jpg' width='200' /></span><o:p></o:p></p></div></div></div>");
                sb.Append("<h3> Sayın " + mesaj + "</h3>");
                sb.Append("  <table style='width:300px;'>" +
                         "<thead>" +
                "<tr ><th>ÜrünKodu</th><th>Miktar</th></tr>" +
                "</thead>" +
                "<tbody>");
                foreach (var item in LUrunKodu)
                {
                    sb.Append("<tr style='border-bottom:1px #000 solid;'><td>" + item.Split('-')[0] + "</td><td style='text-align:right;'>" + item.Split('-')[1] + "</td></tr>");
                }
                sb.Append("</tbody></table>");
                sb.Append("<p>Kodlu Ürünlerinizin Siparişi Sisteme Alınmıştır.İyi Çalışmalar Dileriz.</p>");

                ePosta.IsBodyHtml = true;
                //ePosta.To.Add("info@doganteknik.com");
                ePosta.Subject = GSubject;
                ePosta.Body = sb.ToString();
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new NetworkCredential(GFrom, "01AdanA");
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Host = smtpserver;
                smtp.Send(ePosta);
                return "1";
            }
            catch (Exception ex)
            {
                return "0";
            }

        }

    }
}