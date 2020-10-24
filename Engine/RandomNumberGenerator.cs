﻿using System;
using System.Security.Cryptography;

namespace Engine
{
    public static class RandomNumberGenerator
    {
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();

        public static int NumberBetween(int minimumValue, int maximumValue)
        {
            byte[] randomNumber = new byte[1];
            _generator.GetBytes(randomNumber);
            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
            //need the number to be between 0.0 and 0.999999999 - if the number =1 then it causes rounding issues
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d); //d represents double floating number
            //add 1 to number to allow for math.floor rounding
            int range = maximumValue - minimumValue + 1;
            double randomVlaueInRange = Math.Floor(multiplier * range);

            return (int)(minimumValue + randomVlaueInRange);
        }
    }
}
