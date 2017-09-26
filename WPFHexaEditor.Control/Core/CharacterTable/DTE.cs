//////////////////////////////////////////////
// Apache 2.0  - 2003-2017
// Author : Derek Tremblay (derektremblay666@gmail.com)
//////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace WpfHexaEditor.Core.CharacterTable
{
    /// <summary>
    /// Objet repr�sentant un DTE.
    /// </summary>
    public sealed class Dte : IEquatable<Dte>
    {
        /// <summary>Nom du DTE</summary>
        private string _entry;

        #region Constructeurs

        /// <summary>
        /// Constructeur principal
        /// </summary>
        public Dte()
        {
            _entry = string.Empty;
            Type = DteType.Invalid;
            Value = string.Empty;
        }

        /// <summary>
        /// Contructeur permetant d'ajouter une entr�e et une valeur
        /// </summary>
        /// <param name="entry">Nom du DTE</param>
        /// <param name="value">Valeur du DTE</param>
        public Dte(string entry, string value)
        {
            _entry = entry;
            Value = value;
            Type = DteType.DualTitleEncoding;
        }

        /// <summary>
        /// Contructeur permetant d'ajouter une entr�e, une valeur et une description
        /// </summary>
        /// <param name="entry">Nom du DTE</param>
        /// <param name="value">Valeur du DTE</param>
        /// <param name="type">Type de DTE</param>
        public Dte(string entry, string value, DteType type)
        {
            _entry = entry;
            Value = value;
            Type = type;
        }

        #endregion Constructeurs

        #region Propri�t�s

        /// <summary>
        /// Nom du DTE
        /// </summary>
        public string Entry
        {
            set => _entry = value.ToUpper();
            get => _entry;
        }

        /// <summary>
        /// Valeur du DTE
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Type de DTE
        /// </summary>
        public DteType Type { get; }

        #endregion Propri�t�s

        #region M�thodes

        /// <summary>
        /// Cette fonction permet de retourner le DTE sous forme : [Entry]=[Valeur]
        /// </summary>
        /// <returns>Retourne le DTE sous forme : [Entry]=[Valeur]</returns>
        public override string ToString()
        {
            return Type != DteType.EndBlock &&
                   Type != DteType.EndLine
                ? _entry + "=" + Value
                : _entry;
        }

        #endregion M�thodes

        #region Methodes Static

        public static DteType TypeDte(Dte dteValue)
        {
            try
            {
                switch (dteValue._entry.Length)
                {
                    case 2:
                        if (dteValue.Value.Length == 2)
                            return DteType.Ascii;
                        else
                            return DteType.DualTitleEncoding;

                    case 4: // >2
                        return DteType.MultipleTitleEncoding;
                }
            }
            catch (IndexOutOfRangeException)
            {
                switch (dteValue._entry)
                {
                    case @"/":
                        return DteType.EndBlock;

                    case @"*":
                        return DteType.EndLine;
                        //case @"\":
                }
            }
            catch (ArgumentOutOfRangeException)
            { //Du a une entre qui a 2 = de suite... EX:  XX==
                return DteType.DualTitleEncoding;
            }

            return DteType.Invalid;
        }

        public static DteType TypeDte(string dteValue)
        {
            try
            {
                switch (dteValue)
                {
                    case @"<end>":
                        return DteType.EndBlock;

                    case @"<ln>":
                        return DteType.EndLine;
                        //case @"\":
                }

                if (dteValue.Length == 1)
                    return DteType.Ascii;
                if (dteValue.Length == 2)
                    return DteType.DualTitleEncoding;
                if (dteValue.Length > 2)
                    return DteType.MultipleTitleEncoding;
            }
            catch (ArgumentOutOfRangeException)
            { //Du a une entre qui a 2 = de suite... EX:  XX==
                return DteType.DualTitleEncoding;
            }

            return DteType.Invalid;
        }
        #endregion Methodes Static

        #region IEquatable implementation
        public override bool Equals(object obj) => Equals(obj as Dte);

        public bool Equals(Dte other) => other != null &&
                                         Entry == other.Entry &&
                                         Value == other.Value &&
                                         Type == other.Type;

        public override int GetHashCode()
        {
            var hashCode = -852816310;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Entry);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Value);
            hashCode = hashCode * -1521134295 + Type.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Dte dTe1, Dte dTe2) => EqualityComparer<Dte>.Default.Equals(dTe1, dTe2);

        public static bool operator !=(Dte dTe1, Dte dTe2) => !(dTe1 == dTe2);
        #endregion IEquatable implementation
    }
}