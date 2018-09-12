using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace BentaiDataBase
{
    class BentaiDataBaseHandler
    {
        private static byte[] ImageToByte(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();
                return imageBytes;
            }
        }

        private static Image ByteListToImage(byte[] imageBytes)
        {
            // Convert byte[] to Image
            using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                Image image = new Bitmap(ms);
                return image;
            }
        }

        // If we only want to save one image for some reason
        internal static void SaveImage(Image imageToAdd, Dictionary<string, int> imageTags)
        {
            AddDataBaseEntryWithImage(new Image[] { imageToAdd }, new Dictionary<int, Dictionary<string, int>>() { { 0, imageTags } });
        }

        internal static void AddDataBaseEntryWithImage(Image[] imagesToAdd, Dictionary<int, Dictionary<string, int>> imageTags)
        {
            if (imagesToAdd.Length != imageTags.Keys.ToArray().Length)
            {
                throw new ArgumentException("imageToAdd length is different from imageTags");
            }

            using (SQLiteConnection sqlConnection = new SQLiteConnection(Globals.dataBaseString))
            {
                sqlConnection.Open();

                using (var transaction = sqlConnection.BeginTransaction())
                {
                    for (int i = 0; i < imagesToAdd.Length; i++)
                    {
                        using (SQLiteCommand sqlCommandAdd = new SQLiteCommand(sqlConnection))
                        using (SQLiteCommand sqlCommandGet = new SQLiteCommand(sqlConnection))
                        // Getting the name of the imageId from the db
                        {
                            long imageId;

                            sqlCommandGet.CommandText = "SELECT MAX(imageId) from imageData";
                            using (SQLiteDataReader imageIdReader = sqlCommandGet.ExecuteReader())
                            {
                                if (imageIdReader.Read())
                                {
                                    imageId = (Convert.IsDBNull(imageIdReader[0]) ? 0 : (long)imageIdReader[0] + 1);
                                }
                                else
                                {
                                    throw new Exception("Send help");
                                }
                            }
                            #region Getting imageformat
                            ImageFormat imageFormat;
                            if (ImageFormat.Jpeg.Equals(imagesToAdd[i].RawFormat))
                            {
                                imageFormat = ImageFormat.Jpeg;
                            }
                            else if (ImageFormat.Png.Equals(imagesToAdd[i].RawFormat))
                            {
                                imageFormat = ImageFormat.Png;
                            }
                            else
                            {
                                throw new Exception("Could not find the image format of an image");
                            }
                            #endregion

                            byte[] imageInBytes = ImageToByte(imagesToAdd[i], imageFormat);

                            sqlCommandAdd.CommandText = "insert into imageData(imageId, yuri, loli, kemonomimi, nonh, " +
                            "masturbation, tentacle, solo, toys, bigBreast, boat, blowJob, anal, touhou, ahegao, favorite, image) " +
                            $"values ({imageId}, {imageTags[i]["Yuri"]}, {imageTags[i]["Loli"]}, {imageTags[i]["Kemonomimi"]}, " +
                            $"{imageTags[i]["Non-h"]}, {imageTags[i]["Masturbation"]}, {imageTags[i]["Tentacles"]}, {imageTags[i]["Solo"]}, " +
                            $"{imageTags[i]["Toys"]}, {imageTags[i]["Big Breasts"]}, {imageTags[i]["Boat"]}, {imageTags[i]["BlowJob"]}, " +
                            $"{imageTags[i]["Anal"]}, {imageTags[i]["Touhou"]}, {imageTags[i]["Ahegao"]}, {imageTags[i]["Favorite"]}, @image)";

                            sqlCommandAdd.Parameters.Add("@image", DbType.Binary, imageInBytes.Length);
                            sqlCommandAdd.Parameters["@image"].Value = imageInBytes;

                            sqlCommandAdd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }
        }

        internal static Image GetImage(int imageId)
        {
            using (SQLiteConnection sqlConnection = new SQLiteConnection(Globals.dataBaseString))
            using (SQLiteCommand sqlCommand = new SQLiteCommand(sqlConnection))
            {
                sqlCommand.CommandText = $"SELECT image FROM imageData WHERE imageId = {imageId}";
                sqlConnection.Open();
                using (IDataReader imageReader = sqlCommand.ExecuteReader())
                {
                    Byte[] imageInBytes;
                    if (imageReader.Read())
                    {
                        imageInBytes = (Byte[])imageReader[0];
                        return ByteListToImage(imageInBytes);
                    }
                    else
                    {
                        throw new Exception("send help 2");
                    }
                }
            }
        }
    }
}

