using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace TuringMachineSimulator
{
    // ========== МОДЕЛИ ДЛЯ СЕРИАЛИЗАЦИИ ==========

    [XmlRoot("TuringMachine")]
    public class TuringMachineSaveData
    {
        [XmlElement("InitialState")]
        public string InitialState { get; set; }

        [XmlElement("AcceptState")]
        public string AcceptState { get; set; }

        [XmlArray("Rules")]
        [XmlArrayItem("Rule")]
        public List<RuleSaveData> Rules { get; set; }

        [XmlElement("InitialTapeContent")]
        public string InitialTapeContent { get; set; }

        [XmlElement("InitialHeadPosition")]
        public int InitialHeadPosition { get; set; }
    }

    public class RuleSaveData
    {
        [XmlElement("CurrentState")]
        public string CurrentState { get; set; }

        [XmlElement("ReadSymbol")]
        public char ReadSymbol { get; set; }

        [XmlElement("NewState")]
        public string NewState { get; set; }

        [XmlElement("WriteSymbol")]
        public char WriteSymbol { get; set; }

        [XmlElement("Direction")]
        public char Direction { get; set; }
    }

    // ========== СЕРВИС ДЛЯ РАБОТЫ С XML ==========

    public class TuringMachineXmlService
    {
        private const string DefaultFileName = "turing_machine.xml";
        private const string Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

        public bool SaveToXml(TuringMachine machine, string filePath = null)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    using (var saveDialog = new SaveFileDialog())
                    {
                        saveDialog.Filter = Filter;
                        saveDialog.DefaultExt = "xml";
                        saveDialog.FileName = DefaultFileName;
                        saveDialog.Title = "Сохранить машину Тьюринга";

                        if (saveDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            return false;

                        filePath = saveDialog.FileName;
                    }
                }

                var saveData = new TuringMachineSaveData
                {
                    InitialState = machine.InitialState,
                    AcceptState = machine.AcceptState,
                    Rules = machine.Rules.Select(r => new RuleSaveData
                    {
                        CurrentState = r.CurrentState,
                        ReadSymbol = r.ReadSymbol,
                        NewState = r.NewState,
                        WriteSymbol = r.WriteSymbol,
                        Direction = r.Direction
                    }).ToList(),
                    InitialTapeContent = machine.GetTapeString(),
                    InitialHeadPosition = machine.HeadPosition
                };

                var serializer = new XmlSerializer(typeof(TuringMachineSaveData));
                using (var writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, saveData);
                }

                System.Windows.Forms.MessageBox.Show(
                    $"Файл успешно сохранён!\n{filePath}",
                    "Сохранение",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    $"Ошибка при сохранении: {ex.Message}",
                    "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }

        public bool LoadFromXml(TuringMachine machine, out string errorMessage)
        {
            errorMessage = null;

            try
            {
                using (var openDialog = new OpenFileDialog())
                {
                    openDialog.Filter = Filter;
                    openDialog.DefaultExt = "xml";
                    openDialog.Title = "Открыть машину Тьюринга";

                    if (openDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return false;

                    var serializer = new XmlSerializer(typeof(TuringMachineSaveData));
                    using (var reader = new StreamReader(openDialog.FileName))
                    {
                        var saveData = (TuringMachineSaveData)serializer.Deserialize(reader);

                        // Очищаем текущие правила
                        machine.ClearRules();

                        // Восстанавливаем состояния
                        machine.Configure(saveData.InitialState, saveData.AcceptState);

                        // Восстанавливаем правила
                        foreach (var ruleData in saveData.Rules)
                        {
                            machine.AddRule(new Rule(
                                ruleData.CurrentState,
                                ruleData.ReadSymbol,
                                ruleData.NewState,
                                ruleData.WriteSymbol,
                                ruleData.Direction));
                        }

                        // Восстанавливаем ленту
                        machine.SetTape(saveData.InitialTapeContent, saveData.InitialHeadPosition);
                        machine.SaveCurrentTapeAsInitial();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}