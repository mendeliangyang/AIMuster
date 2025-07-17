using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using AIMuster.Models;
using static System.Net.Mime.MediaTypeNames;

namespace AIMuster.Config
{

    public enum Theme
    {
        Light,
        Dark
    }

    public class AppConfig
    {
        public string Language { get; set; } = "zh";
        public Theme Theme { get; set; } = Theme.Light;
        public int WindowWidth { get; set; } = 800;
        public int WindowHeight { get; set; } = 600;
        public int RowCount { get; set; } = 1;
        public int ColumnCount { get; set; } = 1;
    }

    public static class ConfigManager
    {
        public const string PromptCodeWeb = "$prompt";

        private static readonly string ConfigFilePath = "config.json";
        private static readonly string AiModelConfigFilePath = "AiModelConfig.json";
        private static readonly string ViewAiModelConfigFilePath = "AiViewModelConfig.json";


        private static List<AiModelConfig> aiModelConfigs = new List<AiModelConfig>()
        {
            new AiModelConfig
            {
                ModelName = "DeepSeek",
                ModelId = "DeepSeek",
                ModelType = "DeepSeek",
                ModelVersion = "v1",
                ModelDescription = "DeepSeek",
                ModelUrl = "https://chat.deepseek.com",
                ModelIconUrl = "https://example.com/icon.png",
                IsEnabled = true,
                IsValid = true,
                IsCustomModel = false,
                ObtainElementJs = $@"(() => {{
                        try {{
                                const el = document.getElementById('chat-input');
                                if (!el) return 'element not found';

                                const nativeSetter = Object.getOwnPropertyDescriptor(
                                    el.tagName === 'TEXTAREA' ? HTMLTextAreaElement.prototype
                                                              : HTMLInputElement.prototype,
                                    'value').set;
                                nativeSetter.call(el, '$prompt');

                                el.dispatchEvent(new Event('input',  {{ bubbles:true }}));
                                el.dispatchEvent(new Event('change', {{ bubbles:true }}));

                                return 'success';
                            }} catch(e) {{
                                return 'error: ' + e.message;
                            }}
                        }})()",
                SendElementJs= @"
                                (function() {
                                    const container = document.querySelector('._6f28693');
                                    if (!container) return '未找到 ._6f28693 容器';

                                    const allIcons = container.querySelectorAll('.ds-icon');
                                    let clickedCount = 0;

                                    allIcons.forEach(icon => {
                                        const style = window.getComputedStyle(icon);
                                        if (
                                            style.fontSize === '16px' &&
                                            style.width === '16px' &&
                                            style.height === '16px'
                                        ) {
                                            const event = new MouseEvent('click', {
                                                bubbles: true,
                                                cancelable: true,
                                                view: window
                                            });
                                            icon.dispatchEvent(event);
                                            clickedCount++;
                                        }
                                    });
                                    return clickedCount;
                                })();"
            },
            new AiModelConfig
            {
                ModelName = "ChatGPT",
                ModelId = "ChatGPT",
                ModelType = "OpenAI",
                ModelVersion = "v1",
                ModelDescription = "OpenAI's ChatGPT model",
                ModelUrl = "https://chatgpt.com",
                ModelIconUrl = "https://example.com/icon.png",
                IsEnabled = true,
                IsValid = true,
                IsCustomModel = false,
                ObtainElementJs = $@"
                    (async () => {{
                        try {{
                            const div = document.getElementById('prompt-textarea');
                            if (div) {{
                                const paragraphs = div.getElementsByTagName('p');
                                for (let i = 0; i < paragraphs.length; i++) {{
                                    paragraphs[i].innerText = '$prompt'; 
                                }}
                            }}
                        }} catch (error) {{
                            console.error('Error executing script:', error);
                        }}
                    }})();",
                SendElementJs=@"const button = document.getElementById('composer-submit-button');
                                if (button) {
                                    const event = new MouseEvent('click', {
                                        bubbles: true,
                                        cancelable: true,
                                        view: window
                                    });
                                    button.dispatchEvent(event);
                                    '事件已触发';
                                } else {
                                    '错误：按钮未找到';
                                }"
            },
            new AiModelConfig
            {
                ModelName = "Google",
                ModelId = "Gemini",
                ModelType = "Gemini",
                ModelVersion = "v1",
                ModelDescription = "Google Gemini",
                ModelUrl = "https://gemini.google.com/app",
                ModelIconUrl = "https://example.com/icon.png",
                IsEnabled = true,
                IsValid = true,
                IsCustomModel = false,
                ObtainElementJs = @"
                                (function() {
                                    const div = document.querySelector('.ql-editor.textarea.new-input-ui');
                                    if (!div) return '找不到目标元素';

                                    const p = div.querySelector('p');
                                    if (!p) {
                                        const newP = document.createElement('p');
                                        newP.innerText = '$prompt';
                                        div.appendChild(newP);
                                    } else {
                                        p.innerText = '$prompt';
                                    }

                                    return '内容已更新';
                                })();"
                ,
                SendElementJs=@"
                                (function() {
                                    const button = Array.from(document.querySelectorAll('button')).find(btn =>
                                        btn.classList.contains('send-button') &&
                                        btn.classList.contains('mat-mdc-icon-button') &&
                                        btn.getAttribute('aria-label') === '发送'
                                    );
                                    if (!button) return '未找到发送按钮';

                                    const event = new MouseEvent('click', {
                                        bubbles: true,
                                        cancelable: true,
                                        view: window
                                    });
                                    button.dispatchEvent(event);
                                    return '已触发点击';
                                })();"
            },

            new AiModelConfig
            {
                ModelName = "Grok ",
                ModelId = "Grok",
                ModelType = "Grok",
                ModelVersion = "v1",
                ModelDescription = "Grok",
                ModelUrl = "https://grok.com/",
                ModelIconUrl = "https://example.com/icon.png",
                IsEnabled = true,
                IsValid = true,
                IsCustomModel = false,
                ObtainElementJs = @"
    (function() {
        const textarea = document.querySelector('textarea[aria-label=""Ask Grok anything""]');
        if (textarea) {
            const nativeTextAreaValueSetter = Object.getOwnPropertyDescriptor(window.HTMLTextAreaElement.prototype, 'value').set;
        nativeTextAreaValueSetter.call(textarea, '$prompt');

            const event = new Event('input', { bubbles: true });
            textarea.dispatchEvent(event);

            return 'value set and event dispatched';
        } else {
            return 'textarea not found';
        }
    })();"
                ,
                SendElementJs=@"
    (function() {
        const button = document.querySelector('button[aria-label=""Submit""]');
        if (button && !button.disabled) {
            const event = new MouseEvent('click', {
        bubbles: true,
                cancelable: true,
                view: window
            });
            button.dispatchEvent(event);
            return 'clicked';
        } else {
            return 'not-found-or-disabled';
        }
    })();
"
            },
            new AiModelConfig
            {
                ModelName = "通义",
                ModelId = "tongyi",
                ModelType = "tongyi",
                ModelVersion = "v1",
                ModelDescription = "通义",
                ModelUrl = "https://www.tongyi.com/",
                ModelIconUrl = "https://example.com/icon.png",
                IsEnabled = true,
                IsValid = true,
                IsCustomModel = false,
                ObtainElementJs = @"(()=>{    var element = document.querySelector('textarea[placeholder=""遇事不决问通义""]'); if(!element){    document.querySelector('textarea.ant-input.textarea--FEdqShqI');}     if(element){ const nativeSetter = Object.getOwnPropertyDescriptor(      element.tagName === 'TEXTAREA' ? HTMLTextAreaElement.prototype                               : HTMLInputElement.prototype,      'value').set;  nativeSetter.call(element, '$prompt'); element.dispatchEvent(new Event('input',  { bubbles:true }));  element.dispatchEvent(new Event('change', { bubbles:true }));}})()"
                ,
                SendElementJs=@"
                                (function() {
                                    const el = document.querySelector('.operateBtn--qMhYIdIu');
                                    if (!el) return '未找到目标元素';

                                    const event = new MouseEvent('click', {
                                        bubbles: true,
                                        cancelable: true,
                                        view: window
                                    });

                                    el.dispatchEvent(event);
                                    return '点击事件已触发';
                                })();"
            }
        };


        public static AppConfig LoadAppConfig()
        {
            if (File.Exists(ConfigFilePath))
            {
                var json = File.ReadAllText(ConfigFilePath);
                return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }

            return new AppConfig();
        }

        public static void Save(AppConfig config)
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ConfigFilePath, json);
        }



        public static List<AiModelConfig> LoadAiModelConfig() 
        {
            if (File.Exists(AiModelConfigFilePath))
            {
                var json = File.ReadAllText(AiModelConfigFilePath);
                return JsonSerializer.Deserialize<List<AiModelConfig>>(json) ?? aiModelConfigs;
            }
            return aiModelConfigs;
        }


        public static void SaveAiModelConfig(List<AiModelConfig> configs)
        {
            var json = JsonSerializer.Serialize(configs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(AiModelConfigFilePath, json);
        }


        public static List<AiModelConfig> LoadViewAiModelConfig()
        {
            List<AiModelConfig> viewAiModelConfigs = null;
            if (File.Exists(ViewAiModelConfigFilePath))
            {
                var json = File.ReadAllText(ViewAiModelConfigFilePath);
                viewAiModelConfigs = JsonSerializer.Deserialize<List<AiModelConfig>>(json);
            }
            if (viewAiModelConfigs==null||!viewAiModelConfigs.Any())
            {
                viewAiModelConfigs = aiModelConfigs;
            }
            return viewAiModelConfigs.OrderBy(a => a.RowIndex).ThenBy(a => a.ColumnIndex).ToList();
        }


        public static void SaveViewAiModelConfig(List<AiModelConfig> configs)
        {
            var json = JsonSerializer.Serialize(configs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ViewAiModelConfigFilePath, json);
        }

    }

}
