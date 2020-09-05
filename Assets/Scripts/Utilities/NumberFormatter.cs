using System;
using UnityEngine;

/// <summary>A class which converts numbers to strings. Determines the correct divider (, or .) based on the device's locale.</summary>
public class NumberFormatter
{
    /// <summary>One million as a floating point number.</summary>
    private const double ONE_MILLION = 1e6; //10^6
                                               /// <summary>One billion as a floating point number.</summary>
    private const double ONE_BILLION = 1e9; //10^9
                                                  /// <summary>One trillion as a floating point number.</summary>
    private const double ONE_TRILLION = 1e12; //10^12
                                                      /// <summary>One quadrillion as a floating point number.</summary>
    private const double ONE_QUADRILLION = 1e15; //10^15

    private const double ONE_QUINTILLION = 1e18; //10^18

    private const double ONE_SEXTILLION = 1e21; //10^21

    private const double ONE_SEPTILLION = 1e24; //10^24

    private const double ONE_OCTILLION = 1e27; //10^27

    private const double ONE_NONILLION = 1e30; //10^30

    private const double ONE_DECILLION = 1e33; //10^33

    private const double ONE_UNDECILLION = 1e36; //10^36

    private const double ONE_DUODECILLION = 1e39; //10^39

    private const double ONE_TREDECILLION = 1e42; //10^42

    private const double ONE_QUATTTUOR_DECILLION = 1e45; //10^45

    private const double ONE_QUINDECILLION = 1e48; //10^48

    private const double ONE_SEXDECILLION = 1e51; //10^51

    private const double ONE_SEPTEN_DECILLION = 1e54; //10^54

    private const double ONE_OCTODECILLION = 1e57; //10^57

    private const double ONE_NOVEMDECILLION = 1e60; //10^60

    private const double ONE_VIGINTILLION = 1e63; //10^63

    private const double ONE_CENTILLION = 1e303; //10^303
    
    /*
	Quintillion			10^18
	Sextillion			10^21
	Septillion			10^24
	Octillion			10^27
	Nonillion			10^30
	Decillion			10^33

	Undecillion			10^36
	Duodecillion		10^39
	Tredecillion		10^42
	Quatttuor-decillion	10^45
	Quindecillion		10^48
	Sexdecillion		10^51
	Septen-decillion	10^54
	Octodecillion		10^57
	Novemdecillion		10^60
	Vigintillion		10^63
	Centillion			10^303
	*/

    /// <summary>Converts a number to a string.</summary>
    /// <returns>A string representation of the number.</returns>
    /// <param name="number">The number to convert.</param>
    /// <param name="showDecimalPlaces">If two decimal places should be shown.</param>
    /// <param name="showDollarSign">If the dollar sign should be shown.</param>
    public static string ToString(double number, bool showDecimalPlaces = false, bool showDollarSign = true, bool showSpaceInTheEnd = true)
    {
        if (showDollarSign)
        {
            return string.Format("${0}", ToString(number, showDecimalPlaces, showSpaceInTheEnd));
        }
        else
        {
            return ToString(number, showDecimalPlaces, showSpaceInTheEnd);
        }
    }

    /// <summary>Converts a number to a string.</summary>
    /// <returns>A string representation of the number.</returns>
    /// <param name="number">The number to convert.</param>
    /// <param name="showDecimalPlaces">If two decimal places should be shown.</param>
    private static string ToString(double number, bool showDecimalPlaces = false, bool showSpaceInTheEnd = true)
    {
        double numberToDisplay = number;
        string largeNumberText = "";
        bool showDecimal = false;
        //firstly determine if the number is a large number
        if (number >= ONE_DUODECILLION)
        {
            numberToDisplay = number / ONE_DUODECILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Quadrillion);
            largeNumberText = "Dud";
            showDecimal = false;
        }
        else if (number >= ONE_UNDECILLION)
        {
            numberToDisplay = number / ONE_UNDECILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Quadrillion);
            largeNumberText = "Und";
            showDecimal = false;
        }
        else if (number >= ONE_DECILLION)
        {
            numberToDisplay = number / ONE_DECILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Quadrillion);
            largeNumberText = "De";
            showDecimal = false;
        }
        else if (number >= ONE_NONILLION)
        {
            numberToDisplay = number / ONE_NONILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Quadrillion);
            largeNumberText = "No";
            showDecimal = false;
        }
        else if (number >= ONE_OCTILLION)
        {
            numberToDisplay = number / ONE_OCTILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Quadrillion);
            largeNumberText = "Oc";
            showDecimal = false;
        }
        else if (number >= ONE_SEPTILLION)
        {
            numberToDisplay = number / ONE_SEPTILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Quadrillion);
            largeNumberText = "Sp";
            showDecimal = false;
        }
        else if (number >= ONE_SEXTILLION)
        {
            numberToDisplay = number / ONE_SEXTILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Quadrillion);
            largeNumberText = "Se";
            showDecimal = false;
        }
        else if (number >= ONE_QUINTILLION)
        {
            numberToDisplay = number / ONE_QUINTILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Quadrillion);
            largeNumberText = "Qi";
            showDecimal = false;
        }
        else if (number >= ONE_QUADRILLION)
        {
            numberToDisplay = number / ONE_QUADRILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Quadrillion);
            largeNumberText = "Qu";
            showDecimal = false;
        }
        else if (number >= ONE_TRILLION)
        {
            numberToDisplay = number / ONE_TRILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Trillion);
            largeNumberText = "Tr";
            showDecimal = false;
        }
        else if (number >= ONE_BILLION)
        {
            numberToDisplay = number / ONE_BILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Billion);
            largeNumberText = "Bi";
            showDecimal = true;
        }
        else if (number >= ONE_MILLION)
        {
            numberToDisplay = number / ONE_MILLION;// largeNumberText = LocalizationManager.instance.StringForKey(LocalizationManagerKeys.Billion);
            largeNumberText = "Mi";
            showDecimal = true;
        }
        //format the string depending on if the number to display is whole or not
        bool isWholeNumber = (numberToDisplay == Mathf.Floor((float)numberToDisplay));
        if (showDecimalPlaces && !isWholeNumber)
        {
            if(showSpaceInTheEnd)
                return string.Format("{0:n2} {1}", numberToDisplay, largeNumberText);
            else
                return string.Format("{0:n2}{1}", numberToDisplay, largeNumberText);
        } //if it is a whole number, ignore decimal places i.e. $6, not $6.00
        else
        {
            if (showDecimal)
            {
                if(showSpaceInTheEnd)
                    return string.Format("{0:n2} {1}", numberToDisplay, largeNumberText);
                else
                    return string.Format("{0:n2}{1}", numberToDisplay, largeNumberText);
            }
            else
            {
                if (showSpaceInTheEnd)
                    return string.Format("{0:n0} {1}", numberToDisplay, largeNumberText);
                else
                    return string.Format("{0:n0}{1}", numberToDisplay, largeNumberText);
            }
        }
    }
}
