using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Drawing;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

namespace Shared.Common
{
    public static class Command
    {
        public const int Enable = 1;
        public const int Disable = 0;
        public const bool Active = true;
        public const bool InActive = false;
        public const string MessageSuccess = "Thành công!";
        public const string MessageError = "Có lỗi xảy ra, vui lòng thử lại!";

        #region Helpers
        // http://stackoverflow.com/questions/1097331/enumerate-with-return-type-other-than-string/1097387#1097387
        public static Guid GetGuid(this Enum e)
        {
            Type type = e.GetType();
            MemberInfo[] memInfo = type.GetMember(e.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(EnumGuid), false);
                if (attrs != null && attrs.Length > 0)
                    return ((EnumGuid)attrs[0]).Guid;
            }

            throw new ArgumentException($"Enum {e.ToString()} has no EnumGuid defined!");
        }

        #endregion

        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        public static int CountWords(string text, string separator, int index)
        {
            var array = Regex.Split(text, separator);
            return array[index].Length;
        }

        public static string ConvertToUnSign(string text)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = text.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }

        public static void MoveFile(string sourceFolder, string destinationFolder, string fileName)
        {
            CreateFolder(sourceFolder);
            CreateFolder(destinationFolder);

            var sourceFile = sourceFolder + "\\" + fileName;
            var destinationFile = destinationFolder + "\\" + fileName;

            if (System.IO.File.Exists(sourceFile))
            {
                System.IO.File.Move(sourceFile, destinationFile);
            }
        }

        public static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public static void DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);
        }

        public static void SaveFile(string path, string name, HttpPostedFileBase input)
        {
            CreateFolder(path);//tao thu muc neu chua co
            input.SaveAs(path + "/" + name);
        }

        public static void SaveImage(string path, string imageSave, HttpPostedFileBase filePost)
        {
            CreateFolder(path);//tao thu muc neu chua co
            Image img = Image.FromStream(filePost.InputStream);
            Bitmap bitmapMasterImage = new System.Drawing.Bitmap(img);
            bitmapMasterImage.Save(path + "/" + imageSave);
        }
        public static Image ResizeImage(HttpPostedFileBase filePost, int maxWidth, int maxHeight)
        {
            Image image = Image.FromStream(filePost.InputStream);
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        public static string GetNameImage(HttpPostedFileBase input)
        {
            string type = "." + getFormatFile(input.ContentType).ToString();
            string nameImgDefault = TextClearNormal(input.FileName.Replace(type, ""), 50) + "-" + DateTime.Now.ToString("hhmmssffMMddyy");
            return nameImgDefault + type;
        }

        public static void DeleteImage(string path, string imageFile)
        {
            string paththumb = path + "thumb\\" + imageFile;
            string pathmedium = path + "medium\\" + imageFile;
            string pathlarge = path + imageFile;
            DeleteFile(paththumb);
            DeleteFile(pathmedium);
            DeleteFile(pathlarge);
        }

        public static string getFormatFile(string contentType)
        {
            string format;
            switch (contentType)
            {
                case "image/png":
                    format = "png";
                    break;
                case "image/gif":
                    format = "gif";
                    break;
                case "application/x-shockwave-flash":
                    format = "swf";
                    break;
                default:
                    format = "jpg";
                    break;
            }
            return format;
        }
        public static string GetSiteRoot()
        {
            string port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (port == null || port == "80" || port == "443")
                port = "";
            else
                port = ":" + port;

            string protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (protocol == null || protocol == "0")
                protocol = "http://";
            else
                protocol = "https://";

            string sOut = protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + port + System.Web.HttpContext.Current.Request.ApplicationPath;

            if (sOut.EndsWith("/"))
            {
                sOut = sOut.Substring(0, sOut.Length - 1);
            }

            return sOut;
        }

        public static string toFriendly(string title, int maxLength)//bo dau
        {

            string titleUni = toFriendly1(title);
            var match = Regex.Match(titleUni.ToLower(), "[\\w]+");
            StringBuilder result = new StringBuilder("");
            bool maxLengthHit = false;
            while (match.Success && !maxLengthHit)
            {
                if (result.Length + match.Value.Length <= maxLength)
                {
                    result.Append(match.Value + "-");
                }
                else
                {
                    maxLengthHit = true;
                    // Handle a situation where there is only one word and it is greater than the max length.
                    if (result.Length == 0) result.Append(match.Value.Substring(0, maxLength));
                }
                match = match.NextMatch();
            }
            // Remove trailing '-'
            if (result[result.Length - 1] == '-') result.Remove(result.Length - 1, 1);
            return result.ToString();
        }

        public static string toFriendly1(string ip_str_change)//bo dau
        {
            Regex v_reg_regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string v_str_FormD = ip_str_change.Normalize(NormalizationForm.FormD);
            v_str_FormD = v_str_FormD.Replace(",", "-");
            v_str_FormD = v_str_FormD.Replace(" ", "-");
            v_str_FormD = v_str_FormD.Replace("--", "-");
            return v_reg_regex.Replace(v_str_FormD, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
        }

        public static string toFriendlyForsearch(string ip_str_change)//bo dau
        {
            Regex v_reg_regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string v_str_FormD = ip_str_change.Normalize(NormalizationForm.FormD);
            return v_reg_regex.Replace(v_str_FormD, string.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
        }

        public static int PageTotal(int total, int size)
        {
            int result = 0;
            result = total / size;
            if (total % size > 0)
                result += 1;
            return result;
        }

        public static string TextClear(string text, int length)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > length)
            {
                string cutString = text.Substring(0, length);
                var array = Regex.Split(cutString.Trim(), "\\s+").ToList();
                array.RemoveAt(array.Count() - 1);

                return string.Join(" ", array) + "...";
            }
            else
            {
                return text;
            }
        }
        public static string TextClearNormal(string text, int length)
        {
            string result = "";
            if (text != null && text.Length > length)
            {
                result = text;
                int l = text.Length;
                if (l > length)
                    result = text.Substring(0, length);
            }
            else
            {
                result = text;
            }
            return result;
        }

        public static string RandomCharecter()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public static string EncryptString(string message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes("OriBTech@2014"));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        public static string DecryptString(string Message)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes("OriBTech@2014"));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        public static string ConvertCurrency(string price)
        {
            var length = price.Length;
            if (!string.IsNullOrEmpty(price))
            {
                if (price.Equals("0"))
                {
                    return "Thỏa thuận";
                }
                else if (length > 6 && length <= 9)
                {
                    var result = Double.Parse(price) / 1000000;
                    return string.Format("{0}{1}", result, " triệu");
                }
                else if (length > 9)
                {
                    var result = Double.Parse(price) / 1000000000;
                    return string.Format("{0}{1}", result, " tỷ");
                }
                return string.Format("{0}{1}", Double.Parse(price).ToString("#,###"), " đ");
            }
            return "Thỏa thuận";
        }

        public static string ConvertYoutube(string videoLink)
        {
            string result = videoLink.Substring(videoLink.LastIndexOf("/watch?v=") + 9);
            return string.Format("//www.youtube.com/embed/{0}", result);
        }

        public static string SendMail(string title, string body, string list_Receive)
        {
            //Send mail admin
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("CashMe <duytran1402@gmail.com>");
            string[] to = Regex.Split(list_Receive, ";");
            mailMessage.To.Add(new MailAddress(to[0]));
            for (int i = 1; i < to.Length; i++)
            {
                if (!String.IsNullOrEmpty(to[i]))
                {
                    mailMessage.Bcc.Add(to[i]);
                }
            }
            body += "<span> CashMe - Solution</span><br>";
            mailMessage.Subject = title;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;
            mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
            mailMessage.Priority = MailPriority.High;
            mailMessage.SubjectEncoding = System.Text.Encoding.GetEncoding("utf-8");
            SmtpClient client = new SmtpClient();
            //Add the Creddentials- use your own email id and password
            client.UseDefaultCredentials = false;
            client.Host = "smtp.gmail.com"; //Or Your SMTP Server Address  
            client.Port = 25;
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential("duytran1402@gmail.com", "password");
            try
            {
                client.Send(mailMessage);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        //Image to Base64 String
        public static string ImageToBase64(HttpPostedFileBase filePost, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Image img = Image.FromStream(filePost.InputStream);
                Bitmap bitmapMasterImage = new System.Drawing.Bitmap(img);
                // Convert Image to byte[]
                img.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        //Base64 String to Image
        public static Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }
    }
    public class EnumGuid : Attribute
    {
        public Guid Guid;

        public EnumGuid(string guid)
        {
            Guid = new Guid(guid);
        }
    }

}