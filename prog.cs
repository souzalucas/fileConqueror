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

        public void rmdir(List<string> dir){
            foreach(var d in dir){
                DirectoryInfo di = new DirectoryInfo(d);
                try {
                if (di.Exists) {
                    // Indica que o diretóri já existe
                
                    // Tenta remover o diretório
                    di.Delete(true);
                    Console.WriteLine("Removido com Sucesso.");
                }
                else{
                    Console.WriteLine("O diretorio {0} não existe", d);
                }
            } catch (Exception e) {
                Console.WriteLine("O processo falhou: {0}", e.ToString());
            }
            finally {}
                
            }
        }

        public void mkdir(List<string> dir) {
            Console.WriteLine(dir.ToString());
            foreach(var d in dir){
            Console.WriteLine(d);
            
            DirectoryInfo di = new DirectoryInfo(d);
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
        }

        public void validacao(string comando) {
            string[] palavras = comando.Split(' ');
            
                    List<string> diretorios = new List<string>();
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
                    List<string> parametros = new List<string>();
                    for (int i = 1; i<tamanho; i++) { // percorre o comando e separa os parametros dos diretorios a serem criados
                        if (palavras[i][0] == '-' ) {
                            parametros.Add(palavras[i]);
                        } else {
                            diretorios.Add(palavras[i]);
                        }
                    }
                    if (diretorios.Count > 0) {
                        mkdir(diretorios);
                    } else {
                        // Falta o nome do diretorio
                    }
                    break;
                
                case "rmdir":
                    for (int i = 1; i<tamanho; i++) { // percorre o comando e separa os parametros dos diretorios a serem criados                       
                        diretorios.Add(palavras[i]);
                        }
                    if (diretorios.Count > 0) {
                        rmdir(diretorios);
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