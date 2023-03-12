
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public static class Extensions
{
    #region XML

    public static class SaveLoad
    {
        public static void SaveDataXML(SaveData data, string savepath)
        {
            XmlSerializer ser = new XmlSerializer(typeof(SaveData));
            FileStream fs = new FileStream(savepath, FileMode.Create);
            ser.Serialize(fs, data);
            fs.Close();
            AssetDatabase.Refresh();
        }
        public static SaveData LoadSaveDataFromXML(string savepath)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(SaveData));
                FileStream fs = new FileStream(savepath, FileMode.Open);
                SaveData data = (SaveData)ser.Deserialize(fs);
                fs.Close();

                AssetDatabase.Refresh();
                return data;
            }
            catch (FileNotFoundException e)
            {
                Debug.Log("Save file not found, creating new");
                return null;
            }

        }

    }


    #endregion

    #region tiles

    /// <summary>
    /// Return from top left with desired tile size considered
    /// </summary>
    /// <param name="assignedSpace"></param>
    /// <param name="tileOffsets"></param>
    /// <param name="desiredTileSize"></param>
    /// <returns></returns>
    public static Vector2[] GetTilePositions(RectTransform assignedSpace, Vector2 tileOffsets, Vector2 desiredTileSize)
    {

        var fieldHeight = assignedSpace.rect.height;
        var fieldWidth = assignedSpace.rect.width;

        var _tileWidth = desiredTileSize.x;
        var _tileHeight = desiredTileSize.y;

        float _tileTotalW = _tileWidth + tileOffsets.x;
        float _tileTotalH = _tileHeight + tileOffsets.y;

        int totalHorizontal = ((int)(fieldWidth / _tileTotalW));
        int totalVertical = ((int)(fieldHeight / _tileTotalH));

        Vector2 adjustedOffset = new Vector2(tileOffsets.x, -tileOffsets.y); // move to right and down
        Vector2[] points = new Vector2[totalHorizontal * totalVertical];

        int currentIndex = 0;

        for (int i = 0, j = 0; i < totalVertical; i++)
        {
            while (j < totalHorizontal)
            {
                Vector2 point = new Vector2(adjustedOffset.x + (adjustedOffset.x * j) + (_tileWidth * j), adjustedOffset.y + (adjustedOffset.y * i) + (-_tileHeight * i));
                points[currentIndex] = point;
                j++;
                currentIndex++;
            }
            j = 0;
        }

        currentIndex = 0;
        return points;
    }

    #endregion

    #region random
    public static System.Random Randoms = new System.Random();
    /// <summary>
    /// Return a shuffled list using Fisher-Yates method
    /// </summary>
    /// <typeparam name="T">anything</typeparam>
    /// <param name="input">List<></param>
    /// <returns>same list but mixed</returns>
    public static List<T> ShuffledList<T>(List<T> input)
    {
        for (int i = input.Count - 1; i >= 1; i--)
        {
            int j = Randoms.Next(i + 1);
            T tmp = input[i];
            input[i] = input[j];
            input[j] = tmp;
        }
        return input;
    }
    #endregion






}

