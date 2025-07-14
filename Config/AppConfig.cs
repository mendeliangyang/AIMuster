using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using AIMuster.Models;
using static System.Net.Mime.MediaTypeNames;

namespace AIMuster.Config
{
    public class AppConfig
    {
        public string Language { get; set; } = "zh";
        public string Theme { get; set; } = "Light";
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
        private static readonly string ViewAiModelConfigFilePath = "ViewAiModelConfig.json";


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
                IsDefault = true,
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
                IsDefault = true,
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
            }
            ,
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
                IsDefault = true,
                IsCustomModel = false,
                ObtainElementJs = "(()=>{\r\n    var element = document.querySelector('textarea[placeholder=\"遇事不决问通义\"]');\r\nif(!element)\r\n{\r\n    document.querySelector('textarea.ant-input.textarea--FEdqShqI');\r\n}\r\n     if(element){ const nativeSetter = Object.getOwnPropertyDescriptor(\r\n      element.tagName === 'TEXTAREA' ? HTMLTextAreaElement.prototype\r\n                                : HTMLInputElement.prototype,\r\n      'value').set;\r\n  nativeSetter.call(element, '$prompt');\r\n\r\n  element.dispatchEvent(new Event('input',  { bubbles:true }));\r\n  element.dispatchEvent(new Event('change', { bubbles:true }));}\r\n})()"
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
            if (viewAiModelConfigs==null)
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
