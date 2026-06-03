using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace canserver3.Controls
{
    public class HexTextBox : TextBox
    {
        protected override Type StyleKeyOverride => typeof(TextBox);
        private bool _isProcessingTextChange = false;
        private int _lastCursorPos;
        private bool _wasAddition;
        private bool _wasBackspace;
        private bool _wasDelete;

        public HexTextBox()
        {
            this.AddHandler(TextInputEvent, OnTextInput, RoutingStrategies.Tunnel);
            this.AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
            this.TextChanging += OnTextChanging;
            this.TextChanged += OnTextChanged;
        }

        private void OnTextChanging(object? sender, TextChangingEventArgs e)
        {
            Debug.WriteLine($"changing {this.Text}, {this.CaretIndex}");
        }

        private void OnTextInput(object? sender, TextInputEventArgs e)
        {
            if (!Regex.IsMatch(e.Text ?? "", "^[0-9a-fA-F]*$"))
            {
                e.Handled = true;
            }
            else
            {
                _wasAddition = true;
            }
        }

        private void OnKeyDown(object? sender, KeyEventArgs e)
        {
            _lastCursorPos = this.CaretIndex;
            _wasBackspace = e.Key == Key.Back;
            _wasDelete = e.Key == Key.Delete;

            if (_wasBackspace || _wasDelete)
            {
                _wasAddition = false;

                // 处理在空格位置的删除
                var text = this.Text ?? string.Empty;
                if (_wasBackspace && _lastCursorPos > 0 && text[_lastCursorPos - 1] == ' ')
                {
                    // 删除空格前的一个字符
                    this.Text = text.Remove(_lastCursorPos - 2, 1);
                    this.CaretIndex = _lastCursorPos - 2;
                    e.Handled = true;
                }
                else if (_wasDelete && _lastCursorPos < text.Length && text[_lastCursorPos] == ' ')
                {
                    // 删除空格后的一个字符
                    this.Text = text.Remove(_lastCursorPos + 1, 1);
                    this.CaretIndex = _lastCursorPos;
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.Left || e.Key == Key.Right ||
                e.Key == Key.Home || e.Key == Key.End)
            {
                return;
            }

            if (e.Key < Key.D0 || e.Key > Key.F ||
                (e.Key > Key.D9 && e.Key < Key.A) ||
                (e.Key > Key.Z))
            {
                e.Handled = true;
            }
        }
        private void OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            Debug.WriteLine($"changed {this.Text}, {this.CaretIndex}");
            if (_isProcessingTextChange) return;

            _isProcessingTextChange = true;

            try
            {
                var text = this.Text ?? string.Empty;
                var cleanText = text.Replace(" ", "");

                // 构建格式化文本
                var formattedText = string.Empty;
                for (int i = 0; i < cleanText.Length; i++)
                {
                    if (i > 0 && i % 2 == 0)
                    {
                        formattedText += " ";
                    }
                    formattedText += cleanText[i];
                }

                if (text != formattedText)
                {
                    this.Text = formattedText;

                    // 计算新光标位置
                    int newCursorPos;

                    if (_wasAddition)
                    {
                        /// 处理在空格位置插入的情况
                        if (_lastCursorPos > 0 && _lastCursorPos < text.Length && text[_lastCursorPos - 1] == ' ')
                        {
                            // 在空格位置插入，应该插入到当前字节对的起始位置
                            int cleanPos = GetCleanTextPosition(_lastCursorPos, text);
                            cleanText = cleanText.Insert(cleanPos, " ");//这里不知道为啥歪打正着就可以了
                            formattedText = FormatHexString(cleanText);

                            // 光标移动到插入字符后，跳过空格
                            newCursorPos = GetFormattedTextPosition(cleanPos + 1, cleanText.Length);
                        }
                        else
                        {
                            // 正常插入逻辑
                            newCursorPos = _lastCursorPos + 1;

                            // 如果刚完成一对字符，需要额外前进（跳过空格）
                            if ((cleanText.Length - 1) % 2 == 0 && cleanText.Length > 0)
                            {
                                newCursorPos += 1;
                            }
                        }
                        // 确保光标不越界
                        newCursorPos = Math.Clamp(newCursorPos, 0, formattedText.Length);
                        this.CaretIndex = newCursorPos;
                    }
                    //        else if (_wasBackspace)
                    //        {
                    //            Debug.WriteLine(this.CaretIndex);
                    //            // 修正后的退格逻辑
                    //            if (_lastCursorPos > 0)
                    //            {
                    //                int cleanPosBeforeDelete = 0;
                    //                if (_lastCursorPos % 2 == 0)
                    //                {
                    //                    cleanPosBeforeDelete = GetCleanTextPosition(_lastCursorPos, text) - 1;
                    //                }
                    //                else
                    //                {
                    //                    cleanPosBeforeDelete = GetCleanTextPosition(_lastCursorPos, text);
                    //                }
                    //                // 执行删除操作
                    //                if (cleanPosBeforeDelete > 0)
                    //                {
                    //                    formattedText = FormatHexString(cleanText);
                    //                    int newCleanPos = cleanPosBeforeDelete;
                    //                    // 特殊处理：如果删除后位于字节对的末尾，光标应在空格后
                    //                    if (newCleanPos > 0 && newCleanPos % 2 == 0)
                    //                    {
                    //                        newCursorPos = newCleanPos + (newCleanPos / 2);
                    //                    }
                    //                    else
                    //                    {
                    //                        newCursorPos = newCleanPos + (newCleanPos / 2) + 1;
                    //                    }

                    //                }
                    //                else
                    //                {
                    //                    newCursorPos = 0;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                newCursorPos = 0;
                    //            }
                    //        }
                    //        else if (_wasDelete)
                    //        {
                    //            newCursorPos = _lastCursorPos;
                    //        }
                    //        else
                    //        {
                    //            // 其他情况（如粘贴）
                    //            newCursorPos = formattedText.Length;
                    //        }


                }
            }
            finally
            {
                _isProcessingTextChange = false;
                _wasAddition = false;
                _wasBackspace = false;
                _wasDelete = false;
            }
        }
        private string FormatHexString(string cleanText)
        {
            var formattedText = string.Empty;
            for (int i = 0; i < cleanText.Length; i++)
            {
                if (i > 0 && i % 2 == 0)
                {
                    formattedText += " ";
                }
                formattedText += cleanText[i];
            }
            return formattedText;
        }
        // 将格式化文本中的位置映射到cleanText中的位置
        private int GetCleanTextPosition(int formattedPos, string formattedText)
        {
            int cleanPos = 0;
            for (int i = 0; i < formattedPos && i < formattedText.Length; i++)
            {
                if (formattedText[i] != ' ')
                {
                    cleanPos++;
                }
            }
            return cleanPos;
        }

        // 将cleanText中的位置映射回格式化文本中的位置
        private int GetFormattedTextPosition(int cleanPos, int cleanLength)
        {
            // 每两个字符加一个空格
            return cleanPos + (cleanPos / 2);
        }
    }
}
