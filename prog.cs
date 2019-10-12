using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
namespace Shell
{
    class Terminal {
        string path = Directory.GetCurrentDirectory()+"/"; // obtendo diretório atual

        public void execucao() {

        }

        public bool substituir(string frase, string dir) {
            do {
                Console.Out.NewLine = "";
                Console.WriteLine(frase, dir, (""));
                string opcao = Console.ReadLine();

                if (opcao.Equals("s") || opcao.Equals("S")) {
                    Console.Out.NewLine = "\n";
                    return true;
                } else if (opcao.Equals("n") || opcao.Equals("N")) {
                    Console.Out.NewLine = "\n";
                    return false;                               
                }
            } while (true);
        }

        public void move(List<string> diretorios)
        {
            try
            {   
                string dir1 = Path.GetFullPath(diretorios[0]);
                string dir2 = Path.GetFullPath(diretorios[1]);
                Console.WriteLine(dir1);
                Console.WriteLine(dir2);

                if (Directory.Exists(dir1)){
                    if(Directory.Exists(dir2)){
                        Console.WriteLine("Diretorio /{0} Ja existe deseja exclui-lo [S/N]", dir2);
                        string opcao = Console.ReadLine();
                        if(opcao == "S" || opcao =="s"){
                            Directory.Delete(dir2);
                            Directory.Move(dir1, dir2);

                        }
                    }else{
                        Directory.Move(dir1, dir2);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("O processo falhou: {0}", e.ToString());
            }
        }
        public void rmfile(List<string> arquivos){
            foreach(var arq in arquivos){
                try {
                    if (File.Exists(path+arq)) {
                        // Tenta remover o arquivo
                        File.Delete(path+arq);
                        Console.WriteLine("Arquivo /{0} removido com Sucesso.", path+arq);
                    }
                    else{
                        Console.WriteLine("O arquivo /{0} não existe.", path+arq);
                    }
                } catch (Exception e) {
                    Console.WriteLine("O processo falhou: {0}", e.ToString());
                }
            }
        }

        // Função que remove os arquivos do diretório e seus subdiretórios recursivamente
        public void removeRec(DirectoryInfo diretorio) {
            // Remove cada arquivo da pasta
            foreach (FileInfo fi in diretorio.GetFiles()) {
                fi.Delete();
            }
            // Remove cada subdiretório utilizando recursão
            foreach (DirectoryInfo subDir in diretorio.GetDirectories()) {
                // DirectoryInfo nextAlvoSubDir = alvo.CreateSubdirectory(subDir.Name);
                removeRec(subDir);
            }
        }

        public void rmdir(List<string> diretorios){
            foreach(var dir in diretorios){
                try {
                    if (Directory.Exists(path+dir)) {
                        DirectoryInfo remover = new DirectoryInfo(path+dir);
                        removeRec(remover); // Remove arquivos antes de excluí-lo
                        remover.Delete(true);
                        Console.WriteLine("Diretório /{0} removido com Sucesso.", path+dir);
                    } else {
                        Console.WriteLine("O diretório /{0} não existe.", path+dir);
                    }
                } catch (Exception e) {
                    Console.WriteLine("O processo falhou: {0}", e.ToString());
                }
            }
        }

        public void mkdir(List<string> diretorios) {
            foreach(var dir in diretorios){
                try {
                    if (Directory.Exists(path+dir)) { // Diretório ja existe
                        if (substituir("O diretório /{0} já existe. Deseja substituí-lo? [S/N] >> ", path+dir) ) {
                            // Remove diretório e cria novo
                            List<string> remove = new List<string>(); // O argumento da função rmdir deve ser um List<string>
                            remove.Add(dir);
                            rmdir(remove); // chama a função criada para remover o diretório
                            Directory.CreateDirectory(path+dir);
                            Console.WriteLine("Diretório /{0} criado com sucesso.", path+dir);
                        }
                    } else { // Tenta criar o diretório
                        Directory.CreateDirectory(path+dir);
                        Console.WriteLine("Diretório /{0} criado com sucesso.", path+dir);
                    }
                } catch (Exception e) {
                    Console.WriteLine("O processo falhou: {0}", e.ToString());
                }
            }
        }
        
        public void mkfile(List<string> arquivos){
            foreach(var arq in arquivos){
                try {
                    if(File.Exists(path+arq)){
                        if (substituir("O arquivo /{0} já existe. Deseja substituí-lo? [S/N] >> ", path+arq) ) {
                            File.Delete(path+arq);
                            File.Create(path+arq);
                        }
                    } else{
                        File.Create(path+arq);
                        Console.WriteLine("Arquivo /{0} criado com sucesso.", path+arq);
                    }
                } catch (Exception e) {
                    Console.WriteLine("O processo falhou: {0}", e.ToString());
                }
            }
        }

        // Função que copia os arquivos de uma pasta origem para pasta destino
        public void copyPP(DirectoryInfo fonte, DirectoryInfo alvo) {
            // Se o diretório origem for o mesmo que o diretório destino
            if (fonte.FullName.ToLower() == alvo.FullName.ToLower()) {
                Console.WriteLine("Diretório de origem igual o de destino");
                return;
            }

            // Checa se o diretório destino existe, se não, cria-o
            if (!(Directory.Exists(alvo.FullName))) {
                Directory.CreateDirectory(alvo.FullName);
            }

            // Copia cada arquivo para a pasta destino
            foreach (FileInfo fi in fonte.GetFiles()) {
                if(File.Exists(alvo.FullName+"/"+fi.Name)){
                    if (substituir("O diretório /{0} já existe. Deseja substituí-lo? [S/N] >> ", alvo.FullName+"/"+fi.Name) ) {
                        fi.CopyTo(Path.Combine(alvo.ToString(), fi.Name), true);
                    }
                } else {
                    fi.CopyTo(Path.Combine(alvo.ToString(), fi.Name), true);
                }
            }

            // Copia cada subdiretório utilizando recursão
            foreach (DirectoryInfo diFonteSubDir in fonte.GetDirectories()) {
                if(Directory.Exists(alvo.FullName+"/"+diFonteSubDir.Name)){
                    if (substituir("O diretório /{0} já existe. Deseja substituí-lo? [S/N] >> ", alvo.FullName+"/"+diFonteSubDir.Name) ) {
                        DirectoryInfo nextAlvoSubDir = alvo.CreateSubdirectory(diFonteSubDir.Name);
                        copyPP(diFonteSubDir, nextAlvoSubDir);
                    }
                } else {
                    DirectoryInfo nextAlvoSubDir = alvo.CreateSubdirectory(diFonteSubDir.Name);
                    copyPP(diFonteSubDir, nextAlvoSubDir);
                }
            }
        }

        public void copy(List<string> diretorios, string destino) {

            // Se houver mais de 1 diretório a ser copiado, todos devem ser arquivos
            if (diretorios.Count > 1) {
                foreach(var dir in diretorios) {
                    string fi = path+dir;
                    if(!File.Exists(fi)) {
                        Console.WriteLine("Erro de sintaxe no comando");
                        return;
                    }
                }
            }

            foreach(var or in diretorios){
                string origem = path+or; // concatenando com diretorio atual

                if (File.Exists(origem)) { // Origem é arquivo
                    FileInfo fi = new FileInfo(origem);

                    if (File.Exists(destino)) { // Se destino for um arquivo já existente
                        if (substituir("O arquivo /{0} já existe. Deseja substituí-lo? [S/N] >> ", destino) ) {
                            File.Delete(destino);
                            fi.CopyTo(Path.Combine(destino, fi.Name), true);
                        }
                    } else {
                        if (!Directory.Exists(destino)) { // Se o diretório destino não existir, cria-o
                            Directory.CreateDirectory(destino);
                        }
                        if (File.Exists(destino+"/"+fi.Name)) { // Se arquivo destino já existe
                            if (substituir("O arquivo /{0} já existe. Deseja substituí-lo? [S/N] >> ", destino+"/"+fi.Name) ) {
                                fi.CopyTo(Path.Combine(destino, fi.Name), true);
                            }
                        }
                        else {
                            fi.CopyTo(Path.Combine(destino, fi.Name), true);
                        }
                    } 
                
                } else if (Directory.Exists(origem)){ // Origem é pasta

                    if(File.Exists(destino)) {
                        Console.WriteLine("Impossível copiar diretório para um arquivo");
                    } else {
                        DirectoryInfo pastaDestino = new DirectoryInfo(destino);
                        DirectoryInfo pastaOrigem = new DirectoryInfo(origem);
                        // Chama a função copiando a pasta e seus subdiretórios para o destino
                        copyPP(pastaOrigem, pastaDestino);
                    }
                } else {
                    Console.WriteLine("Diretório /{0} não existe", origem);
                }
            }
        }

        // Remonta a string de caminho se houver .. (ponto-ponto)
        public string arrumaString(string[] divide) {
            string caminho = "";
            List<string> list = new List<string>();

            foreach (var div in divide) {
                if (div.Equals("..")) {
                    list.Remove(list[list.Count-1]);
                } else {
                    list.Add(div);
                }
            }
            foreach (var li in list) {
                caminho = caminho+li+"/";
            }
            return caminho;
        }

        public void cd(string caminho) {
            if (caminho[0] != '/') { // caminho relativo
                caminho = path+caminho; // concatena diretório atual + caminho fornecido
            }
            if (Directory.Exists(caminho)) { // caminho existe
                string[] divide = caminho.Split('/');
                path = arrumaString(divide);

            } else { // caminho não existe
                Console.WriteLine("O diretório {0} não existe", caminho);
            }
        }

        public void validacao(string comando) {
            string[] palavras = comando.Split(' '); // separando as palavras do comando

            List<string> diretorios = new List<string>();
            int tamanho = palavras.Length; // quantidade de palavras no comando (comando + argumentos)

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

                case "move":
                    for (int i = 1; i<tamanho; i++) { // percorre o comando e separa os parametros dos arquivos a serem criados                       
                        diretorios.Add(palavras[i]);
                        }
                    if (diretorios.Count > 0) {
                        move(diretorios);
                    } else {
                        // Falta o nome do arquivo
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

                case "mkfile":
                    for (int i = 1; i<tamanho; i++) { // percorre o comando e separa os parametros dos diretorios a serem criados                       
                        diretorios.Add(palavras[i]);
                        }
                    if (diretorios.Count > 0) {
                        mkfile(diretorios);
                    } else {}
                    break;


                case "rmfile":
                    for (int i = 1; i<tamanho; i++) { // percorre o comando e separa os parametros dos arquivos a serem criados                       
                        diretorios.Add(palavras[i]);
                        }
                    if (diretorios.Count > 0) {
                        rmfile(diretorios);
                    } else {
                        // Falta o nome do arquivo
                    }
                    break;
                
                case "copy":
                    if (tamanho >= 3) {
                        int i;
                        for (i = 1; i<tamanho-1; i++) { // percorre o comando e separa os parametros dos arquivos/diretorios a serem copiados                       
                            diretorios.Add(palavras[i]);
                        }
                        string destino = palavras[i];
                        copy(diretorios, path+destino);
                    } else {
                        Console.WriteLine("Falta de argumentos");
                    }
                    
                    break;

                case "cd":
                    if (tamanho != 2) {
                        Console.WriteLine("Número inválido de argumentos");
                    } else {
                        cd(palavras[1]);
                    }   
                    break;
                
            }
        }

        public void principal() {
            while(true) {
                Console.Out.NewLine = "";
                Console.WriteLine("user@machine:~{0}$ ", path, (""));
                string comando = Console.ReadLine();
                Console.Out.NewLine = "\n";
                this.validacao(comando);

            }
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