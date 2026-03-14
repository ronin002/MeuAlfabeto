import os
import re

def renomear_arquivos():
    # Pega o caminho absoluto da pasta onde este script .py está salvo
    diretorio_do_script = os.path.dirname(os.path.abspath(__file__))
    
    # Padrão para encontrar " (numero)"
    padrao = re.compile(r'\s\((\d+)\)')

    print(f"Iniciando a renomeação na pasta do script: {diretorio_do_script}")

    for nome_arquivo in os.listdir(diretorio_do_script):
        # Ignora o próprio script para não renomear a si mesmo por acidente
        if nome_arquivo == os.path.basename(__file__):
            continue

        busca = padrao.search(nome_arquivo)
        
        if busca:
            numero = busca.group(1)
            numero_formatado = numero.zfill(2)
            
            novo_nome = padrao.sub(f'_{numero_formatado}', nome_arquivo)
            
            caminho_antigo = os.path.join(diretorio_do_script, nome_arquivo)
            caminho_novo = os.path.join(diretorio_do_script, novo_nome)
            
            try:
                os.rename(caminho_antigo, caminho_novo)
                print(f"Sucesso: '{nome_arquivo}' -> '{novo_nome}'")
            except Exception as e:
                print(f"Erro ao renomear '{nome_arquivo}': {e}")

if __name__ == "__main__":
    renomear_arquivos()