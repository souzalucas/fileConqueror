using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace Shell {
    class Terminal {
        string path = Directory.GetCurrentDirectory()+"/"; // obtendo diretório atual

        // Função genérica que confirma substituição de arquivo/diretório
        public bool substituir(string frase, string dir) {
            do {
                Console.Out.NewLine = "";
                Console.WriteLine(frase, dir, (""));
                string opcao = Console.ReadLine();

                if(opcao.Equals("s") || opcao.Equals("S")) {
                    Console.Out.NewLine = "\n";
                    return true;
                } else if(opcao.Equals("n") || opcao.Equals("N")) {
                    Console.Out.NewLine = "\n";
                    return false;                               
                }
            } while(true);
        }

        public void move(List<string> diretorios) {
            try {   
                string dir1 = Path.GetFullPath(diretorios[0]);
                string dir2 = Path.GetFullPath(diretorios[1]);

                if(Directory.Exists(dir1)) {
                    if(Directory.Exists(dir2)) {
                        if(substituir("O diretório /{0} Ja existe deseja exclui-lo [S/N] >> ", dir2)) {
                            Directory.Delete(dir2);
                            Directory.Move(dir1, dir2);
                        }
                    } else {
                        Directory.Move(dir1, dir2);
                    }
                } else if(File.Exists(dir1)) {
                    if(File.Exists(dir2)) {
                        if(substituir("O arquivo /{0} Ja existe deseja exclui-lo [S/N] >> ", dir2)) {
                            File.Delete(dir2);
                            File.Move(dir1, dir2);
                        }
                    } else {
                        File.Move(dir1, dir2);
                    }
                } else {
                    Console.WriteLine("O arquivo/diretório {0} não existe", dir1);
                }
            } catch {
                Console.WriteLine("O processo falhou, leia o manual");
            }
        }

        public void rmfile(List<string> arquivos) {
            foreach(var arq in arquivos) {
                try {
                    if(File.Exists(path+arq)) {
                        // Tenta remover o arquivo
                        File.Delete(path+arq);
                        Console.WriteLine("Arquivo /{0} removido com Sucesso.", path+arq);
                    } else {
                        Console.WriteLine("O arquivo /{0} não existe.", path+arq);
                    }
                } catch(Exception e) {
                    Console.WriteLine("O processo falhou: {0}", e.ToString());
                }
            }
        }

        // Função que remove os arquivos do diretório e seus subdiretórios recursivamente
        public void removeRec(DirectoryInfo diretorio) {
            // Remove cada arquivo da pasta
            foreach(FileInfo fi in diretorio.GetFiles()) {
                fi.Delete();
            }
            // Remove cada subdiretório utilizando recursão
            foreach(DirectoryInfo subDir in diretorio.GetDirectories()) {
                // DirectoryInfo nextAlvoSubDir = alvo.CreateSubdirectory(subDir.Name);
                removeRec(subDir);
            }
        }

        public void ls(List<string> argumentos){
            if(argumentos.Count() > 2) {
                Console.WriteLine("Numero maximo de argumentos é 2");
                return;
            }
            List<string> output = new List<string>();
            var dirs =  Directory.EnumerateDirectories(path);
            var files = Directory.EnumerateFiles(path);
            if(argumentos.Count() == 0) {

                foreach(var dir in dirs) {
                    if(dir[dir.LastIndexOf('/')+1]!= '.') {
                        output.Add(dir.Substring(dir.LastIndexOf('/')+1));
                    }
                }
                foreach(var fi in files) {
                    if(fi[fi.LastIndexOf('/')+1] !='.')
                    output.Add(fi.Substring(fi.LastIndexOf('/')+1));
                }
                foreach( var o in output) {
                    Console.WriteLine(o);
                }
                return;

            } else if(argumentos.Count() > 0) {
                switch(argumentos[0]) {
                    case "-valid":
                        foreach(var dir in dirs) {
                            if(dir[dir.LastIndexOf('/')+1]!= '.') {
                                output.Add(dir.Substring(dir.LastIndexOf('/')+1));
                            }
                        }
                        foreach(var fi in files) {
                            if(fi[fi.LastIndexOf('/')+1] !='.') {
                                output.Add(fi.Substring(fi.LastIndexOf('/')+1));
                            }
                        }
                    break;

                    case "-hidden":
                        output.Add("..");
                        output.Add(".");
                        foreach(var dir in dirs) {
                            output.Add(dir.Substring(dir.LastIndexOf('/')+1));
                        }
                        foreach(var fi in files) {
                            output.Add(fi.Substring(fi.LastIndexOf('/')+1));
                        }
                    break;
                    
                    case "-dirs":
                        output.Add("..");
                        output.Add(".");
                        foreach(var dir in dirs) {
                            if(dir[dir.LastIndexOf('/')+1]!= '.') {
                                output.Add(dir.Substring(dir.LastIndexOf('/')+1));
                            }
                        }
                    break;
                    
                    case "-files":
                        output.Add("..");
                        output.Add(".");
                        foreach(var fi in files){
                            if(fi[fi.LastIndexOf('/')+1] !='.') {
                                output.Add(fi.Substring(fi.LastIndexOf('/')+1));
                            }
                        }
                    break;
                    
                    case "-full":
                        output.Add(".. Size 4096 B");
                        output.Add(". Size 4096 B");
                        foreach(var fi in files) {
                            var tam = new FileInfo(fi).Length;
                            var criacao = File.GetCreationTime(fi);
                            DateTime ultimoAcesso = File.GetLastAccessTime(fi);
                            //Console.WriteLine(File.GetCreationTime(fi));
                            output.Add(fi.Substring(fi.LastIndexOf('/')+1) + " Size " + tam+" B Created at " + criacao + " Last Accessed at "+ ultimoAcesso);
                        }
                        foreach(var dir in dirs) {
                            var criacao = Directory.GetCreationTime(dir);
                            DateTime ultimoAcesso = Directory.GetLastAccessTime(dir);
                            //Console.WriteLine(dirle.GetCreationTime(dir));
                            output.Add(dir.Substring(dir.LastIndexOf('/')+1) + " Size 4096" +" B Created at " + criacao + " Last Accessed at "+ ultimoAcesso);
                        }
                    break;
                    
                    case "-sortasc":
                        foreach(var dir in dirs) {
                            output.Add(dir.Substring(dir.LastIndexOf('/')+1));
                        }
                        foreach(var fi in files) {
                            output.Add(fi.Substring(fi.LastIndexOf('/')+1));
                        }
                        output.Sort();
                    break;

                    case "-sortdesc":
                        foreach(var dir in dirs) {
                            output.Add(dir.Substring(dir.LastIndexOf('/')+1));
                        }
                        foreach(var fi in files) {
                            output.Add(fi.Substring(fi.LastIndexOf('/')+1));
                        }
                        output.Sort();
                        output.Reverse();
                    break;

                    default:
                        Console.WriteLine("Primeiro argumento invalido");
                    return;
                }
                if(argumentos.Count() == 2) {
                    switch(argumentos[1]) {
                        case "-sortasc":
                            output.Sort();
                        break;
                        
                        case "-sortdesc":
                            output.Sort();
                            output.Reverse();
                        break;
                        
                        default:
                            Console.WriteLine("Segundo argumento invalido");
                        return;  
                    }
                }   
                
            }
            foreach(var saida in output) {
                Console.WriteLine(saida);
            }
        }

        public void rmdir(List<string> diretorios) {
            foreach(var dir in diretorios) {
                try {
                    if(Directory.Exists(path+dir)) {
                        DirectoryInfo remover = new DirectoryInfo(path+dir);
                        removeRec(remover); // Remove arquivos antes de excluí-lo
                        remover.Delete(true);
                        Console.WriteLine("Diretório /{0} removido com Sucesso.", path+dir);
                    } else {
                        Console.WriteLine("O diretório /{0} não existe.", path+dir);
                    }
                } catch(Exception e) {
                    Console.WriteLine("O processo falhou: {0}", e.ToString());
                }
            }
        }

        public void mkdir(List<string> diretorios) {
            foreach(var dir in diretorios){
                try {
                    if(Directory.Exists(path+dir)) { // Diretório ja existe
                        if(substituir("O diretório /{0} já existe. Deseja substituí-lo? [S/N] >> ", path+dir)) {
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
                } catch(Exception e) {
                    Console.WriteLine("O processo falhou: {0}", e.ToString());
                }
            }
        }
        
        public void mkfile(List<string> arquivos) {
            foreach(var arq in arquivos) {
                try {
                    if(File.Exists(path+arq)) {
                        if(substituir("O arquivo /{0} já existe. Deseja substituí-lo? [S/N] >> ", path+arq)) {
                            File.Delete(path+arq);
                            File.Create(path+arq);
                        }
                    } else {
                        File.Create(path+arq);
                        Console.WriteLine("Arquivo /{0} criado com sucesso.", path+arq);
                    }
                } catch(Exception e) {
                    Console.WriteLine("O processo falhou: {0}", e.ToString());
                }
            }
        }

        // Função que copia os arquivos de uma pasta origem para pasta destino
        public void copyPP(DirectoryInfo origem, DirectoryInfo destino) {
            // Se o diretório origem for o mesmo que o diretório destino
            if(origem.FullName.ToLower() == destino.FullName.ToLower()) {
                Console.WriteLine("Diretório de origem igual o de destino");
                return;
            }

            // Checa se o diretório destino existe, se não, cria-o
            if(!(Directory.Exists(destino.FullName))) {
                Directory.CreateDirectory(destino.FullName);
            }

            // Copia cada arquivo para a pasta destino
            foreach(FileInfo fi in origem.GetFiles()) {
                if(File.Exists(destino.FullName+"/"+fi.Name)) {
                    if(substituir("O arquivo /{0} já existe. Deseja substituí-lo? [S/N] >> ", destino.FullName+"/"+fi.Name)) {
                        fi.CopyTo(Path.Combine(destino.ToString(), fi.Name), true);
                    }
                } else {
                    fi.CopyTo(Path.Combine(destino.ToString(), fi.Name), true);
                }
            }

            // Copia cada subdiretório utilizando recursão
            foreach(DirectoryInfo diOrigemSubDir in origem.GetDirectories()) {
                if(Directory.Exists(destino.FullName+"/"+diOrigemSubDir.Name)) {
                    if(substituir("O diretório /{0} já existe. Deseja substituí-lo? [S/N] >> ", destino.FullName+"/"+diOrigemSubDir.Name)) {
                        DirectoryInfo nextDestinoSubDir = destino.CreateSubdirectory(diOrigemSubDir.Name);
                        copyPP(diOrigemSubDir, nextDestinoSubDir);
                    }
                } else {
                    DirectoryInfo nextDestinoSubDir = destino.CreateSubdirectory(diOrigemSubDir.Name);
                    copyPP(diOrigemSubDir, nextDestinoSubDir);
                }
            }
        }

        public void copy(List<string> diretorios, string destino) {

            if(File.Exists(destino)) {
                Console.WriteLine("Impossível copiar para um arquivo");
                return;
            }

            foreach(var or in diretorios) {
                string origem = path+or; // concatenando com diretorio atual

                if(File.Exists(origem)) { // Origem é arquivo
                    FileInfo fi = new FileInfo(origem);

                    if(!Directory.Exists(destino)) { // Se o diretório destino não existir, cria-o
                        Directory.CreateDirectory(destino);
                    }
                    if(File.Exists(destino+"/"+fi.Name)) { // Se arquivo destino já existe
                        if(substituir("O arquivo /{0} já existe. Deseja substituí-lo? [S/N] >> ", destino+"/"+fi.Name)) {
                            fi.CopyTo(Path.Combine(destino, fi.Name), true);
                        }
                    } else {
                        fi.CopyTo(Path.Combine(destino, fi.Name), true);
                    }
                
                } else if(Directory.Exists(origem)) { // Origem é pasta

                    DirectoryInfo pastaDestino = new DirectoryInfo(destino);
                    DirectoryInfo pastaOrigem = new DirectoryInfo(origem);
                    // Chama a função copiando a pasta e seus subdiretórios para o destino
                    copyPP(pastaOrigem, pastaDestino);
                } else {
                    Console.WriteLine("Diretório /{0} não existe", origem);
                }
            }
        }

        // Remonta a string de caminho
        public string arrumaString(string caminho) {
            if(caminho[caminho.Length-1].Equals('/')) { // tira o '/' do final, se houver
                caminho = caminho.Remove(caminho.Length-1);
            }
            string[] divide = caminho.Split('/');
            caminho = "";
            List<string> list = new List<string>();

            // Itera por todo o caminho
            foreach(var div in divide) { 
                if(div.Equals("..")) { // Caso houver .. (ponto-ponto)
                    list.Remove(list[list.Count-1]); // remove ultimo diretorio da lista
                } else { // se não
                    list.Add(div); // adiciona diretorio na lista
                }
            }

            // Monta todo o caminho em uma só string
            foreach(var li in list) {
                caminho = caminho+li+"/";
            }
            return caminho;
        }

        public void cd(string caminho) {
            if(caminho[0] != '/') { // caminho relativo
                caminho = path+caminho; // concatena diretório atual + caminho fornecido
            }
            if(Directory.Exists(caminho)) { // caminho existe
                path = arrumaString(caminho); // tira os .. (ponto-ponto) do caminho

            } else { // caminho não existe
                Console.WriteLine("O diretório {0} não existe", caminho);
            }
        }

        public void cat(string arquivo) {
            try {
                if(File.Exists(path+arquivo)) { // Se o arquivo existe
                    string texto = File.ReadAllText(path+arquivo); // Lê conteúdo do arquivo
                    Console.WriteLine(texto); // Imprime conteúdo na tela
                } else {
                    Console.WriteLine("{0} não existe", arquivo);
                }
            } catch(Exception e) {
                Console.WriteLine("O processo falhou: {0}", e.ToString());
            }
        }

        public void locate(DirectoryInfo substring, string alvo, string caminho) {

            // Passa por todos os arquivos do diretório e verifica se é o procurado
            foreach(FileInfo arquivo in substring.GetFiles()) {
                if((arquivo.Name).Equals(alvo)) {
                    Console.WriteLine(caminho+arquivo.Name); // Retorna o caminho do arquivo
                }
            }

            // Passa por todos os subdiretórios recursivamente
            foreach(DirectoryInfo subDir in substring.GetDirectories()) {
                if((subDir.Name).Equals(alvo)) { // Verifica se o diretório em questão é o procurado
                    Console.WriteLine(caminho+subDir.Name); // Retorna o caminho do diretório
                } else {
                    locate(subDir, alvo, caminho+subDir.Name+"/");
                }
            }
        }

        public void man(string comando) {
            if(comando.Equals("")) {
                Console.WriteLine("Este software é um interpretador de comandos para sistemas unix, os comandos disponiveis são: cat, cd, clear, copy, locate, ls, man, mkdir, mkfile, move, rmdir, rmfile\n Para saber como usar esses comandos utilize man nomeDoComando");
            } else {
                cat("manual/"+comando);
            }
        }

        public void validacao(string comando) {
            string[] palavras = comando.Split(' '); // separando as palavras do comando
            List<string> parametros = new List<string>();
            List<string> diretorios = new List<string>();
            int tamanho = palavras.Length; // quantidade de palavras no comando (comando + argumentos)

            switch(palavras[0]) {
                case "ls":
                    for(int i = 1; i<tamanho; i++) {
                        if(palavras[i][0] == '-' ) {
                            parametros.Add(palavras[i]);
                        }
                    }
                    ls(parametros);
                break;

                case "mkdir":
                    for(int i = 1; i<tamanho; i++) {
                        diretorios.Add(palavras[i]);
                    }
                    if(diretorios.Count > 0) {
                        mkdir(diretorios);
                    } else {
                        Console.WriteLine("Especifique os diretórios a serem criados");
                    }
                break;  

                case "move":
                    for(int i = 1; i<tamanho; i++) {                      
                        diretorios.Add(palavras[i]);
                    }
                    if(diretorios.Count > 0) {
                        move(diretorios);
                    } else {
                        Console.WriteLine("Especifique o diretório/arquivo a ser movido");
                    }
                break;
                
                case "rmdir":
                    for(int i = 1; i<tamanho; i++) {                    
                        diretorios.Add(palavras[i]);
                    }
                    if(diretorios.Count > 0) {
                        rmdir(diretorios);
                    } else {
                        Console.WriteLine("Especifique os diretórios a serem removidos");
                    }
                break;

                case "mkfile":
                    for(int i = 1; i<tamanho; i++) {                       
                        diretorios.Add(palavras[i]);
                    }
                    if(diretorios.Count > 0) {
                        mkfile(diretorios);
                    } else {
                        Console.WriteLine("Especifique os arquivos a serem criados");
                    }
                break;

                case "rmfile":
                    for(int i = 1; i<tamanho; i++) {                       
                        diretorios.Add(palavras[i]);
                    }
                    if(diretorios.Count > 0) {
                        rmfile(diretorios);
                    } else {
                        Console.WriteLine("Especifique os arquivos a serem removidos");
                    }
                break;
                
                case "copy":
                    if(tamanho >= 3) {
                        int i;
                        for(i = 1; i<tamanho-1; i++) {                       
                            diretorios.Add(palavras[i]);
                        }
                        string destino = palavras[i];
                        copy(diretorios, path+destino);
                    } else {
                        Console.WriteLine("Número inválido de argumentos");
                    }
                break;

                case "cd":
                    if(tamanho != 2) {
                        Console.WriteLine("Número inválido de argumentos");
                    } else {
                        cd(palavras[1]);
                    }   
                break;

                case "cat":
                    if(tamanho != 2) {
                        Console.WriteLine("Número inválido de argumentos");
                    } else {
                        cat(palavras[1]);
                    }   
                break;

                case "locate":
                    if(tamanho != 2) {
                        Console.WriteLine("Número inválido de argumentos");
                    } else {
                        DirectoryInfo diretorioAtual = new DirectoryInfo(path);
                        locate(diretorioAtual, palavras[1], path);
                    }   
                break;

                case "clear":
                    if(tamanho != 1) {
                        Console.WriteLine("Esse comando não deve possuir argumentos");
                    } else {
                        try {
                            Console.Clear();
                        } catch(Exception e) {
                            Console.WriteLine("O processo falhou: {0}", e.ToString());
                        }
                    }
                break;

                case "man":
                    if(tamanho == 1) {
                        man("");
                    } else if(tamanho == 2) {
                        man(palavras[1]);
                    } else {
                        Console.WriteLine("Número inválido de argumentos");
                    }
                break;

                default:
                    Console.WriteLine("Comando inexistente");
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
        }
    } 

}