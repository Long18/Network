using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectBrowser
{
    public static class FindHelper
    {
        private const int ADJACENCY_BONUS = 3; // bonus for adjacent matches
        private const int SEPARATOR_BONUS = 10; // bonus if match occurs after a separator
        private const int CAMEL_BONUS = 10; // bonus if match is uppercase and prev is lower

        private const int
            LEADING_LETTER_PENALTY = -3; // penalty applied for every letter in stringToSearch before the first match

        private const int MAX_LEADING_LETTER_PENALTY = -9; // maximum penalty for leading letters
        private const int UNMATCHED_LETTER_PENALTY = -1; // penalty for every letter that doesn't matter

        // Loop variables
        private static int _score = 0;
        private static int _patternIndex = 0;
        private static int _patternLength;
        private static int _stringIndex = 0;
        private static int _stringLength;
        private static bool _previousMatched = false;
        private static bool _previousLower = false;

        private static bool _isPreviousSeparator = true; // true if first letter match gets separator bonus

        // Score consts
        private static char? _bestLetter;
        private static char? _bestLower;
        private static int? _bestLetterIdx;
        private static int _bestLetterScore;

        private static readonly List<int> _matchedIndices = new();

        public static void Match(string stringToSearch, string pattern, out int outScore)
        {
            _patternLength = pattern.Length;
            _stringLength = stringToSearch.Length;

            // Loop over strings
            while (_stringIndex != _stringLength)
            {
                char? patternChar = _patternIndex != _patternLength ? pattern[_patternIndex] as char? : null;
                char strChar = stringToSearch[_stringIndex];

                char? patternLower = patternChar != null ? char.ToLower((char)patternChar) as char? : null;
                char strLower = char.ToLower(strChar);
                char strUpper = char.ToUpper(strChar);

                bool nextMatch = patternChar != null && patternLower == strLower;
                bool rematch = _bestLetter != null && _bestLower == strLower;

                bool advanced = nextMatch && _bestLetter != null;
                bool patternRepeat = _bestLetter != null && patternChar != null && _bestLower == patternLower;

                if (advanced || patternRepeat)
                {
                    _score += _bestLetterScore;
                    _matchedIndices.Add((int)_bestLetterIdx);
                    _bestLetter = null;
                    _bestLower = null;
                    _bestLetterIdx = null;
                    _bestLetterScore = 0;
                }

                if (nextMatch || rematch)
                {
                    int newScore = 0;

                    // Apply penalty for each letter before the first pattern match
                    // Note: Math.Max because penalties are negative values. So max is smallest penalty.
                    if (_patternIndex == 0)
                    {
                        var penalty = Mathf.Max(_stringIndex * LEADING_LETTER_PENALTY, MAX_LEADING_LETTER_PENALTY);
                        _score += penalty;
                    }

                    // Apply bonus for consecutive bonuses
                    if (_previousMatched) newScore += ADJACENCY_BONUS;

                    // Apply bonus for matches after a separator
                    if (_isPreviousSeparator) newScore += SEPARATOR_BONUS;

                    // Apply bonus across camel case boundaries. Includes "clever" isLetter check.
                    if (_previousLower && strChar == strUpper && strLower != strUpper)
                        newScore += CAMEL_BONUS;

                    // Update pattern index IF the next pattern letter was matched
                    if (nextMatch) ++_patternIndex;

                    // Update best letter in stringToSearch which may be for a "next" letter or a "rematch"
                    if (newScore >= _bestLetterScore)
                    {
                        // Apply penalty for now skipped letter
                        if (_bestLetter != null)
                            _score += UNMATCHED_LETTER_PENALTY;

                        _bestLetter = strChar;
                        _bestLower = char.ToLower((char)_bestLetter);
                        _bestLetterIdx = _stringIndex;
                        _bestLetterScore = newScore;
                    }

                    _previousMatched = true;
                }
                else
                {
                    _score += UNMATCHED_LETTER_PENALTY;
                    _previousMatched = false;
                }

                // Includes "clever" isLetter check.
                _previousLower = strChar == strLower && strLower != strUpper;
                _isPreviousSeparator = strChar == '_' || strChar == ' ';

                ++_stringIndex;
            }

            // Apply score for last match
            if (_bestLetter == null)
            {
                _score += _bestLetterScore;
                _matchedIndices.Add((int)_bestLetterIdx);
            }

            outScore = _score;
        }
    }
}