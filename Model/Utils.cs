using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TrackingJob.Model
{
    class Utils
    {
		// encrypt
		public static String sha256_hash ( String value )
		{
			StringBuilder Sb = new StringBuilder();

			using ( SHA256 hash = SHA256Managed.Create() ) {
				Encoding enc = Encoding.UTF8;
				Byte[] result = hash.ComputeHash( enc.GetBytes( value ) );

				foreach ( Byte b in result )
					Sb.Append( b.ToString( "x2" ) );
			}

			return Sb.ToString();
		}

        //
        public static string EncryptString ( string key, string plainText )
        {
            byte[] iv = new byte[16];
            byte[] array;

            using ( Aes aes = Aes.Create() ) {
                aes.Key = Encoding.UTF8.GetBytes( key );
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor( aes.Key, aes.IV );

                using ( MemoryStream memoryStream = new MemoryStream() ) {
                    using ( CryptoStream cryptoStream = new CryptoStream( ( Stream ) memoryStream, encryptor, CryptoStreamMode.Write ) ) {
                        using ( StreamWriter streamWriter = new StreamWriter( ( Stream ) cryptoStream ) ) {
                            streamWriter.Write( plainText );
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String( array );
        }

        public static string DecryptString ( string key, string cipherText )
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String( cipherText );

            using ( Aes aes = Aes.Create() ) {
                aes.Key = Encoding.UTF8.GetBytes( key );
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor( aes.Key, aes.IV );

                using ( MemoryStream memoryStream = new MemoryStream( buffer ) ) {
                    using ( CryptoStream cryptoStream = new CryptoStream( ( Stream ) memoryStream, decryptor, CryptoStreamMode.Read ) ) {
                        using ( StreamReader streamReader = new StreamReader( ( Stream ) cryptoStream ) ) {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
