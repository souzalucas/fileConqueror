using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Shell
{
    class Terminal {
        
        public void execucao() {

        }

        public void mkdir(List<string> dir) {
            DirectoryInfo di = new DirectoryInfo(dir[0]);
            try {
                if (di.Exists) {
                    // Indica que o diretóri já existe
                    Console.WriteLine("Este diretório já existe.");
                    return;
                }
                // Tenta criar o diretório
                di.Create();
                Console.WriteLine("Diretório criado com sucesso.");
            } catch (Exception e) {
                Console.WriteLine("O processo falhou: {0}", e.ToString());
            }
            finally {}
        }

        public void validacao(string comando) {
            string[] palavras = comando.Split(' ');
            
            int tamanho = palavras.Length;

            switch (palavras[0]) {
                case "ls":
                    
                    if((tamanho > 1) && (palavras[1][0] == '-')) {
                        // é um parametro 
                    } else {
                        // é um diretório
                        DirectoryInfo di = new DirectoryInfo(@"novo");
                        // Console.WriteLine("{0}", di);
                        di.Create();
                    }
                        
                    break;

                case "mkdir":
                    List<string> diretorios = new List<string>();
                    List<string> parametros = new List<string>();
                    int i = 1;
                    while (i < 3) { // percorre o comando e separa os parametros dos diretorios a serem criados
                        if (palavras[i][0] == '-' ) {
                            parametros.Add(palavras[i]);
                        } else {
                            diretorios.Add(palavras[i]);
                        }
                        i++;
                    }
                    if (diretorios.Count > 0) {
                        mkdir(diretorios);
                    } else {
                        // Falta o nome do diretorio
                    }
                    break;
            }
        }

        public void principal() {
            // while()true {
                Console.WriteLine(">>");
                string comando = Console.ReadLine();

                this.validacao(comando);

            // }
        }
    }

    class Program {
        static void Main() {
            Terminal T = new Terminal();
            T.principal();
            // Console.WriteLine("Hello World!");
            // Keep the console window open in debug mode.
            // Console.WriteLine("Press any key to exit.");
            // Console.ReadKey();
        }
    }

} 