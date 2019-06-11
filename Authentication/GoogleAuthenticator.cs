using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Jiper.Authentication
{
    public class GoogleAuthenticator
    {
        public static string GetToken(string secret)
        {
            try
            {
                var secretBytes = Base32String.Instance.Decode(secret);
                var passGenenerator = new PasscodeGenerator(new HMACSHA1(secretBytes));
                var timeoutCode = passGenenerator.GenerateTimeoutCode();
                return timeoutCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
    }

    public class Base32String
    {
        #region Fields
        private const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567"; //RFC 4668/3548
        private static readonly Base32String INSTANCE = new Base32String(ALPHABET);

        private readonly int[] _lookup = null;

        private char[] DIGITS;
        private readonly int MASK;
        private readonly int SHIFT;
        private Dictionary<char, int> CHAR_MAP;

        private const string SEPARATOR = "-";
        #endregion

        #region Singleton and constructors
        /// <summary>
        /// Gets the instance of singleton class.
        /// </summary>
        public static Base32String Instance
        {
            get
            {
                return Base32String.INSTANCE;
            }
            private set
            {

            }
        }
        /// <summary>
        /// Prevents a default instance of the <see cref="Base32String"/> class from being created.
        /// </summary>
        /// <param name="alphabet">The alphabet.</param>
        private Base32String(string alphabet)
        {
            DIGITS = ALPHABET.ToCharArray();
            MASK = DIGITS.Length - 1;
            _lookup = new int[37] {
        32, 0, 1, 26, 2, 23, 27, 0, 3, 16, 24, 30, 28, 11, 0, 13, 4, 7, 17,
        0, 25, 22, 31, 15, 29, 10, 12, 6, 0, 21, 14, 9, 5, 20, 8, 19, 18
            };
            SHIFT = NumberOfTrailingZeros(DIGITS.Length);
            CHAR_MAP = new Dictionary<char, int>();
            for (var i = 0; i < DIGITS.Length; i++)
            {
                CHAR_MAP.Add(DIGITS[i], i);
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Encodes the specified byte array to base32 string.
        /// </summary>
        /// <param name="data">The byte array containing data.</param>
        /// <returns></returns>
        public string Encode(byte[] data)
        {
            return Base32String.Instance.EncodeInternal(data);
        }

        /// <summary>
        /// Decodes base32 encoded string.
        /// </summary>
        /// <param name="encoded">The encoded string.</param>
        /// <returns></returns>
        public byte[] Decode(string encoded)
        {
            return Base32String.Instance.DecodeInternal(encoded);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Gets the number the of trailing zeros.
        /// </summary>
        /// <param name="i">The i.</param>
        /// <returns></returns>
        private int NumberOfTrailingZeros(int i)
        {
            return _lookup[(-i & i) % 37];
        }

        /// <summary>
        /// Decodes base32 encoded string.
        /// </summary>
        /// <param name="encoded">The encoded string.</param>
        /// <returns></returns>
        private byte[] DecodeInternal(string encoded)
        {
            encoded = encoded.Trim().Replace(SEPARATOR, "").Replace(" ", "");
            encoded = encoded.ToUpper();
            if (encoded.Length == 0)
                return new byte[0];
            var encodedLength = encoded.Length;
            var outLength = encodedLength * SHIFT / 8;
            var result = new byte[outLength];
            var buffer = 0;
            var next = 0;
            var bitsLeft = 0;
            foreach (var c in encoded.ToCharArray())
            {
                if (!CHAR_MAP.ContainsKey(c))
                    throw new FormatException("Illegal character: " + c);
                buffer <<= SHIFT;
                buffer |= CHAR_MAP[c] & MASK;
                bitsLeft += SHIFT;
                if (bitsLeft >= 8)
                {
                    result[next++] = (byte)(buffer >> (bitsLeft - 8));
                    bitsLeft -= 8;
                }
            }
            return result;
        }

        /// <summary>
        /// Encodes the specified byte array to base32 string..
        /// </summary>
        /// <param name="data">The byte array containing data.</param>
        /// <returns></returns>
        private string EncodeInternal(byte[] data)
        {
            if (data.Length == 0)
                return "";
            if (data.Length >= (1 << 28))
                throw new ArgumentException();

            var outputLength = (data.Length * 8 + SHIFT - 1) / SHIFT;
            var result = new StringBuilder(outputLength);

            int buffer = data[0];
            var next = 1;
            var bitsLeft = 8;
            while (bitsLeft > 0 || next < data.Length)
            {
                if (bitsLeft < SHIFT)
                {
                    if (next < data.Length)
                    {
                        buffer <<= 8;
                        buffer |= (data[next++] & 0xFF);
                        bitsLeft += 8;
                    }
                    else
                    {
                        var pad = SHIFT - bitsLeft;
                        buffer <<= pad;
                        bitsLeft += pad;
                    }
                }
                var index = MASK & (buffer >> (bitsLeft - SHIFT));
                bitsLeft -= SHIFT;
                result.Append(DIGITS[index]);
            }
            return result.ToString();
        }
        #endregion
    }

    public class PasscodeGenerator
    {
        #region Fields
        private const int PASS_CODE_LENGTH = 6;
        private const int INTERVAL = 30;
        private const int ADJECENT_INTERVALS = 1;
        private readonly int PIN_MODULO = (int)Math.Pow(10, PASS_CODE_LENGTH);

        private readonly Func<byte[], byte[]> signer;
        private readonly int codeLength;
        private readonly int intervalPeriod;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="PasscodeGenerator"/> class.
        /// </summary>
        /// <param name="sha1">The instance of HMAC SHA1.</param>
        public PasscodeGenerator(HMACSHA1 sha1)
        {
            this.signer = (e) =>
            {
                return sha1.ComputeHash(e);
            };
            this.codeLength = PASS_CODE_LENGTH;
            this.intervalPeriod = INTERVAL;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasscodeGenerator"/> class.
        /// </summary>
        /// <param name="sha1">The instance of HMAC SHA1.</param>
        /// <param name="passCodeLength">Length of the decimal passcode.</param>
        /// <param name="interval">The interval passcode is valid for.</param>
        public PasscodeGenerator(HMACSHA1 sha1, int passCodeLength, int interval)
        {
            this.codeLength = passCodeLength;
            this.intervalPeriod = interval;
            this.signer = (e) =>
            {
                return sha1.ComputeHash(e);
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PasscodeGenerator"/> class.
        /// </summary>
        /// <param name="signer">The signer delegate function.</param>
        /// <param name="passCodeLength">Length of the decimal passcode.</param>
        /// <param name="interval">The interval passcode is valid for.</param>
        public PasscodeGenerator(Func<byte[], byte[]> signer, int passCodeLength, int interval)
        {
            this.signer = signer;
            this.codeLength = passCodeLength;
            this.intervalPeriod = interval;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Pads the output.
        /// </summary>
        /// <param name="value">The passcode value.</param>
        /// <returns></returns>
        private string PadOutput(int value)
        {
            var result = value.ToString();
            for (var i = result.Length; i < this.codeLength; i++)
            {
                result = "0" + result;
            }
            return result;
        }

        /// <summary>
        /// Extracts positive integer value from the input array starting at the given offset.
        /// </summary>
        /// <param name="bytes">The array of bytes.</param>
        /// <param name="start">The starting point of extraction.</param>
        /// <returns></returns>
        private int HashToInt(byte[] bytes, int start)
        {
            return (((bytes[start] & 0xFF) << 24) | ((bytes[start + 1] & 0xFF) << 16) |
                ((bytes[start + 2] & 0xFF) << 8) | (bytes[start + 3] & 0xFF)
                );
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Generates the timeout code.
        /// </summary>
        /// <returns></returns>
        public string GenerateTimeoutCode()
        {
            return GenerateResponseCode(this.Clock);
        }

        /// <summary>
        /// Generates the response code.
        /// </summary>
        /// <param name="challange">The challange value.</param>
        /// <returns></returns>
        public string GenerateResponseCode(long challange)
        {
            var challangeBytes = BitConverter.GetBytes(challange);
            // Must be big endian (according to RFC 4226)
            if (BitConverter.IsLittleEndian)
                // If this runs on little endian system - reverse bytes
                Array.Reverse(challangeBytes, 0, challangeBytes.Length);
            return GenerateResponseCode(challangeBytes);
        }

        /// <summary>
        /// Generates the response code.
        /// </summary>
        /// <param name="challange">The challange value as byte array.</param>
        /// <returns></returns>
        public string GenerateResponseCode(byte[] challange)
        {
            var hash = this.signer.Invoke(challange);
            var offset = hash[hash.Length - 1] & 0xF;
            var truncatedHash = HashToInt(hash, offset) & 0x7FFFFFFF;
            var pinValue = truncatedHash % PIN_MODULO;
            return PadOutput(pinValue);
        }

        /// <summary>
        /// Verifies the response code.
        /// </summary>
        /// <param name="challange">The challange value.</param>
        /// <param name="response">The response value.</param>
        /// <returns></returns>
        public bool VerifyResponseCode(long challange, string response)
        {
            var extectedResponse = GenerateResponseCode(challange);
            return extectedResponse.Equals(response);
        }

        /// <summary>
        /// Verifies the timeout code.
        /// </summary>
        /// <param name="timeoutCode">The timeout code.</param>
        /// <returns></returns>
        public bool VerifyTimeoutCode(string timeoutCode)
        {
            return VerifyTimeoutCode(timeoutCode, ADJECENT_INTERVALS, ADJECENT_INTERVALS);
        }

        /// <summary>
        /// Verifies the timeout code.
        /// </summary>
        /// <param name="timeoutCode">The timeout code.</param>
        /// <param name="pastIntervals">The number of past intervals to check.</param>
        /// <param name="futureIntervals">The number of future intervals to check.</param>
        /// <returns></returns>
        public bool VerifyTimeoutCode(string timeoutCode, int pastIntervals, int futureIntervals)
        {
            var currentInterval = this.Clock;
            var extectedResponse = GenerateResponseCode(currentInterval);
            if (extectedResponse.Equals(timeoutCode))
            {
                return true;
            }
            for (var i = 1; i < pastIntervals; i++)
            {
                var pastResponse = GenerateResponseCode(currentInterval - i);
                if (pastResponse.Equals(timeoutCode))
                    return true;
            }
            for (var i = 1; i < futureIntervals; i++)
            {
                var futureResponse = GenerateResponseCode(currentInterval + i);
                if (futureIntervals.Equals(timeoutCode))
                    return true;
            }
            return false;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the interval value in milliseconds starting from the Unix epoch (1970-01-01T00:00:00Z ISO 8601)
        /// </summary>
        /// <value>
        /// Interval the code is valid for (in milliseconds starting from Unix epoch).
        /// </value>
        private long Clock
        {
            get
            {
                //Epoch time value
                var epoch = new DateTime(1970, 1, 1);
                //Milliseconds passed Unix epoch.
                var currentTimeMillis = (long)(DateTime.UtcNow - epoch).TotalMilliseconds / 1000;
                return currentTimeMillis / this.intervalPeriod;
            }
            set
            {
                throw new Exception("No assignment allowed");
            }
        }
        #endregion
    }

}
