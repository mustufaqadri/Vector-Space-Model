using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// HASH MAP
using System.Collections;
// FOR FILING
using System.IO;

// Regexr
using System.Text.RegularExpressions;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Vector_Space_Model
{
    class VectorSpace
    {
        public Text TermList;

        // Dictionary is equivalent to Hashmap of JAVA
        public Dictionary<string,Dictionary<int,double>> Table;
        public Dictionary<string, double> IDFTable;
        public Dictionary<int, LinkedList<double>> VectorTable;
        public Dictionary<int, double> DocMagntude;
        public Dictionary<string,double> QueryTable;

        public String Temp;
        public String VectorSpaceString;
        public String Query;
        public String Result;
        public String LexiconSize;
        public int[] FileMatrix;
        public double[] CosineFinal;
        public String ResultCount;
        public VectorSpace()
        {
            Table = new Dictionary<string,Dictionary<int,double>>();
            TermList = new Text();
            //TermList.CreateLexicon();
            TermList.RetrieveLexicon();
        }
        public void ConstructVectorSpace()
        {
            String[] NewStr;
            double TF_IDF = 0.0;
            int DF = 0;
            double TF = 0.0;
            double IDF = 0.0;
            int i;
            int[] x = new int[10826];

            x[0] = 0;
            i = 1;
            NewStr = TermList.FinalString.Split(' ');
            foreach (String word in NewStr)
            {
                DF = 0;
                for (int FileNo = 1; FileNo < 51; FileNo++)
                {
                    if (TermList.ArrangedFile[FileNo].Contains(" " + word + " ") == true)
                    {
                        DF++;
                    }
                }
                x[i] = DF;
                i++;
            }
            i = 1;

            foreach (String word in NewStr)
            {
                Dictionary<int, double> Posting = new Dictionary<int,double>();
                
                TF_IDF = 0.0;
                IDF = 0.0;
                TF = 0.0;
                for (int FileNo = 1; FileNo < 51; FileNo++)
                {
                    if (TermList.ArrangedFile[FileNo].Contains(" " + word + " ") == true)
                    {
                        if (Table.ContainsKey(word) == false)
                        {
                            String[] New = TermList.ArrangedFile[FileNo].Split(' ');
                            TF_IDF = 0.0;
                            IDF = 0.0;
                            TF = 0.0;
                            foreach (String word2 in New )
                            {
                                if(" "+word2+" "==" "+word+" ")
                                {
                                    TF++;
                                }
                            }
                            IDF = Math.Log10(50.0/x[i]);
                            TF_IDF = Math.Round(TF * IDF, 5);
                            Posting.Add(FileNo, TF_IDF);
                            Table.Add(word,Posting);
                            if(TF_IDF>0.0)
                            {
                                Temp += FileNo.ToString() + "(" + TF_IDF.ToString() + ") ";
                            }
                        }
                        else
                        {
                            String[] New = TermList.ArrangedFile[FileNo].Split(' ');
                            TF_IDF = 0.0;
                            IDF = 0.0;
                            TF = 0.0;
                            foreach (String word2 in New)
                            {
                                if (" " + word2 + " " == " " + word + " ")
                                {
                                    TF++;
                                }
                            }
                            IDF = Math.Log10(50.0 / x[i]);
                            TF_IDF = Math.Round(TF * IDF,5);
                            Posting.Add(FileNo, TF_IDF);
                            if (TF_IDF > 0.0)
                            {
                                Temp += FileNo.ToString() + "(" + TF_IDF.ToString() + ") ";
                            }
                        }
                    }
                    else
                    {
                        TF = 0;
                        if(x[i]!=0)
                        {
                            IDF = Math.Log10(50.0 / x[i]);
                        }
                        TF_IDF = Math.Round(TF * IDF, 5);
                        Posting.Add(FileNo, TF_IDF);
                        if (Table.ContainsKey(word) == false)
                        {
                            Table.Add(word, Posting);
                        }
                        if (TF_IDF > 0.0)
                        {
                            Temp += FileNo.ToString() + "(" + TF_IDF.ToString() + ") ";
                        }
                    }
                }
                VectorSpaceString = VectorSpaceString + word +" : \n" + Temp + "\n\n" + "-----------------------------------------------"+"\n\n";
                Temp = "";
                i++;
            }
            // Lexicon Size 
            int ChCount = 0, WordCount = 0;
            int Len = TermList.FinalString.Length;
            while (ChCount < Len)
            {
                if (TermList.FinalString[ChCount] == ' ')
                {
                    WordCount++;
                }
                ChCount++;
            }
            WordCount++;
            LexiconSize = WordCount.ToString();
            // Writing Inverted Index on file
            File.WriteAllText("Vector Space.txt", VectorSpaceString);
        }
        public void CreateIDFTable()
        {
            IDFTable = new Dictionary<string, double>();
            int[] DFArray = new int[10826];
            int DF = 0;
            DFArray[0] = 0;
            int i = 1; 
            String[] NewStr = TermList.FinalString.Split(' ');
            foreach (String word in NewStr)
            {
                DF = 0;
                for (int FileNo = 1; FileNo < 51; FileNo++)
                {
                    if (TermList.ArrangedFile[FileNo].Contains(" " + word + " ") == true)
                    {
                        DF++;
                    }
                }
                DFArray[i] = DF;
                i++;
            }
            double IDF = 0.0;
            i = 1;
            foreach(String word in NewStr)
            {
                IDF = Math.Log10(50.0 / DFArray[i]);
                IDF = Math.Round(IDF, 5);
                i++;
                IDFTable.Add(word, IDF);
            }
        }
        public void CreateVectorTable()
        {
            VectorTable = new Dictionary<int, LinkedList<double>>();
            String[] NewStr = TermList.FinalString.Split(' ');
            double TF = 0.0;
            double IDF = 0.0;
            double TFIDF = 0.0;
            int DocFreq = 0;
            int[] DF = new int[10826];
            DF[0] = 0;
            int i = 1;
            foreach (String word in NewStr)
            {
                DocFreq = 0;
                for (int FileNo = 1; FileNo < 51; FileNo++)
                {
                    if (TermList.ArrangedFile[FileNo].Contains(" " + word + " ") == true)
                    {
                        DocFreq++;
                    }
                }
                DF[i] = DocFreq;
                i++;
            }
            int C = 0;
            for (int FileNo = 1; FileNo < 51; FileNo++)
            {
                LinkedList<double> Post = new LinkedList<double>();
                String[] New = TermList.ArrangedFile[FileNo].Split(' ');
                i = 1;
                foreach (String word1 in NewStr)
                {
                    TF = 0;
                    foreach (String word2 in New)
                    {
                        if (word2==word1)
                        {
                            TF++;
                        }
                    }
                    IDF = Math.Log10(50.0 / DF[i]);
                    TFIDF = Math.Round(TF * IDF, 5);
                    if (VectorTable.ContainsKey(FileNo) == false)
                    {
                        Post.AddLast(TFIDF);
                        VectorTable.Add(FileNo,Post);
                        C++;
                    }
                    else
                    {
                        Post.AddLast(TFIDF);
                        C++;
                    }
                    i++;
                }
            }
        }
        public void PrintTable()
        {
            //Temp="";
            Result = "";
           
            int C = 0;
            foreach (var kvp in DocMagntude)
            {
                //LinkedList<double>.Enumerator It = kvp.Value.GetEnumerator();
                //It.MoveNext();
                //{
                    Result = Result + kvp.Key + " : " + kvp.Value + "\n";
                    C++;
               // }
                
                //break;
            } 
        }
        /*
        foreach (var kvp in Table)
        {
            Dictionary<int, double> Temp = new Dictionary<int, double>();
            Temp = kvp.Value;
            String Temp6 = "";
            foreach(var Post in Temp)
            {
                Temp6 = Temp6 + Post.Key + "(" + Post.Value + ")" + " ";
            }
            Result = Result + kvp.Key + " : " + Temp6 + "\n";
        }*/
     
        public void CreateDocMagnitudeTable()
        {
            DocMagntude = new Dictionary<int, double>();
            double Magnitude = 0.0;
            double Holder = 0.0;
            foreach(var kvp in VectorTable)
            {
                Magnitude = 0.0;
                LinkedList<double>.Enumerator It = kvp.Value.GetEnumerator();
                while(It.MoveNext())
                {
                    if (It.Current>0.0 && It.Current<Double.PositiveInfinity)
                    {
                        Holder=It.Current;
                        Holder = Math.Pow(Holder,2);
                        Magnitude += Holder;
                    }
                }
                Magnitude = Math.Sqrt(Magnitude);
                Magnitude = Math.Round(Magnitude, 5);
                DocMagntude.Add(kvp.Key, Magnitude);
            }
        }
        
        public void QueryProcessing()
        {
            Query = Query.ToLower();
            Result = "";

            // Query Words Count
            int ChCount = 0, WordCount = 0;
            int Len = Query.Length;
            while (ChCount < Len)
            {
                if (Query[ChCount] == ' ')
                {
                    WordCount++;
                }
                ChCount++;
            }
            WordCount++;

            if (Query != " " && Query != "\0")
            {
                String[] Keyword;

                MatchCollection MC = Regex.Matches(Query, @"([A-Z]*[a-z]*[0-9]*)\w+");
                Query = "";

                foreach (Match M in MC)
                {
                    Query += M + " ";
                }

                Keyword = Query.Split(' ');
                ArrayList SortedQuery = new ArrayList();
                foreach (String word in Keyword)
                {
                    SortedQuery.Add(word);
                }
                SortedQuery.Sort();
                Keyword = Query.Split(' ');
                QueryTable = new Dictionary<string, double>();

                double TF_IDF = 0.0;
                double TF = 0;
                double IDF = 0.0;
                double QMagnitude = 0.0;
                double Holder = 0.0;

                foreach (var KVP in IDFTable)
                {
                    IDF = 0.0;
                    TF_IDF = 0.0;
                    TF = 0.0;
                    if (SortedQuery.Contains(KVP.Key))
                    {
                        foreach (String Word2 in Keyword)
                        {
                            if (KVP.Key == Word2)
                            {
                                TF++;
                            }
                        }
                        IDF = KVP.Value;
                        TF_IDF = Math.Round(TF * IDF, 5);
                    }
                    QueryTable.Add(KVP.Key, TF_IDF);
                    if (TF_IDF > 0.0 && TF_IDF < Double.PositiveInfinity)
                    {
                        Holder = TF_IDF;
                        Holder = Math.Pow(Holder, 2);
                        QMagnitude += Holder;
                    }
                }
                QMagnitude = Math.Sqrt(QMagnitude);
                QMagnitude = Math.Round(QMagnitude, 5);
                Temp = QMagnitude.ToString();


                double DotProduct = 1.0;
                double TempProduct = 0.0;

                // Vector Multiplcation
                double[] DotArray = new double[51];
                DotArray[0] = 0.0;
                int i = 1;
                Temp = "";
                Result = "";
                foreach(var D in VectorTable)
                {
                    LinkedList<double>.Enumerator It = D.Value.GetEnumerator();
                    DotProduct = 0.0;

                    foreach(var Q in QueryTable)
                    {
                        while(It.MoveNext())
                        {
                            if(It.Current>0.0 &&  Q.Value>0.0 && It.Current < Double.PositiveInfinity)
                            {
                                TempProduct = Q.Value * It.Current;
                                TempProduct = Math.Round(TempProduct, 5);
                                DotProduct += TempProduct;
                                
                                //Temp = Temp + Q.Value + " = " + It.Current + " " +DotProduct+"\n";
                            }
                            break;
                        }
                    }
                    DotArray[i] = DotProduct;
                    i++;
                }

                double[] CosineArray = new double[51];
                CosineFinal = new double[51];
                CosineArray[0] = 0.0;
                i = 1;
                Temp = "";
                foreach(var DocM in DocMagntude)
                {
                    Holder = (double)DotArray[i] / (DocM.Value * QMagnitude);
                    if(Holder>0.0)
                    {
                        //CosineArray[i] = Math.Cos(Holder);
                        CosineArray[i] = Math.Round(Holder, 5);
                    }
                    else
                    {
                        CosineArray[i] = 0.0;
                    }
                    i++;
                }
                //CosineFinal = new double[51];
                FileMatrix = new int[51];
                CosineFinal[0] = 0.0;
                FileMatrix[0] = 0;
                ResultCount = "";
                Result = "";
                Temp = "";
                int Count = 0;
                for(i=1;i<51;i++)
                {
                    if(CosineArray[i]>0.0)  // && CosineArray[i]<=1.0)
                    {
                        Count++;
                        CosineFinal[i] = CosineArray[i];
                        FileMatrix[i] = i;
                        //Temp = Temp + "Doc ID: " + i + " Cosine Similarity : " + CosineArray[i]+"\n"; 
                    }
                }
                //ResultCount=
                Result = Count.ToString();
            }
            else
            {
                Result = "Invalid Query";
            }
        }
    
        public void WriteTable()
        {
            BinaryFormatter formatter;
            formatter = new BinaryFormatter();
            try
            {
                FileStream writerFileStream = new FileStream("VectorSpace.dat", FileMode.Create, FileAccess.Write);
                formatter.Serialize(writerFileStream, Table);
                writerFileStream.Close();
            }
            catch (Exception)
            {
                Temp = "Unable";
            }
        }
        public void WriteIDFTable()
        {
            BinaryFormatter formatter;
            formatter = new BinaryFormatter();
            try
            {
                FileStream writerFileStream = new FileStream("IDFTable.dat", FileMode.Create, FileAccess.Write);
                formatter.Serialize(writerFileStream,IDFTable);
                writerFileStream.Close();
            }
            catch (Exception)
            {
                Temp = "Unable";
            }
        }
        public void WriteVectorTable()
        {
            BinaryFormatter formatter;
            formatter = new BinaryFormatter();
            try
            {
                FileStream writerFileStream = new FileStream("VectorTable.dat", FileMode.Create, FileAccess.Write);
                formatter.Serialize(writerFileStream,VectorTable);
                writerFileStream.Close();
            }
            catch (Exception)
            {
                Temp = "Unable";
            }
        }
        public void WriteMagnitudeTable()
        {
            BinaryFormatter formatter;
            formatter = new BinaryFormatter();
            try
            {
                FileStream writerFileStream = new FileStream("MagnitudeTable.dat", FileMode.Create, FileAccess.Write);
                formatter.Serialize(writerFileStream, DocMagntude);
                writerFileStream.Close();
            }
            catch (Exception)
            {
                Temp = "Unable";
            }
        }

        public void ReadTable()
        {
            // Lexicon Size
            int ChCount = 0, WordCount = 0;
            int Len = TermList.FinalString.Length;
            while (ChCount < Len)
            {
                if (TermList.FinalString[ChCount] == ' ')
                {
                    WordCount++;
                }
                ChCount++;
            }
            WordCount++;
            LexiconSize = WordCount.ToString();

            if (File.Exists("VectorSpace.dat"))
            {
                BinaryFormatter formatter;
                formatter = new BinaryFormatter();
                try
                {
                    FileStream readerFileStream = new FileStream("VectorSpace.dat", FileMode.Open, FileAccess.Read);
                    Table = (Dictionary<String,Dictionary<int,double>>)formatter.Deserialize(readerFileStream);
                    readerFileStream.Close();
                    VectorSpaceString = File.ReadAllText("Vector Space.txt");
                }
                catch (Exception)
                {
                    Temp = "Unable";
                }
            }
        }
        public void ReadIDFTable()
        {
            if (File.Exists("IDFTable.dat"))
            {
                BinaryFormatter formatter;
                formatter = new BinaryFormatter();
                try
                {
                    FileStream readerFileStream = new FileStream("IDFTable.dat", FileMode.Open, FileAccess.Read);
                    IDFTable = (Dictionary<String,double>)formatter.Deserialize(readerFileStream);
                    readerFileStream.Close();
                }
                catch (Exception)
                {
                    Temp = "Unable";
                }
            }
        }
        public void ReadVectorTable()
        {
            if (File.Exists("VectorTable.dat"))
            {
                BinaryFormatter formatter;
                formatter = new BinaryFormatter();
                try
                {
                    FileStream readerFileStream = new FileStream("VectorTable.dat", FileMode.Open, FileAccess.Read);
                    VectorTable = (Dictionary<int,LinkedList<double>>)formatter.Deserialize(readerFileStream);
                    readerFileStream.Close();
                }
                catch (Exception)
                {
                    Temp = "Unable";
                }
            }
        }
        public void ReadMagnitudeTable()
        {
            if (File.Exists("MagnitudeTable.dat"))
            {
                BinaryFormatter formatter;
                formatter = new BinaryFormatter();
                try
                {
                    FileStream readerFileStream = new FileStream("MagnitudeTable.dat", FileMode.Open, FileAccess.Read);
                    DocMagntude = (Dictionary<int, double>)formatter.Deserialize(readerFileStream);
                    readerFileStream.Close();
                }
                catch (Exception)
                {
                    Temp = "Unable";
                }
            }
        }
    }
}