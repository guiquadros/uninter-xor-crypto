Guilherme Quadros da Silva

RU: 3282910

# Uninter - Matemática Computacional: AP Criptografia Simétrica com XOR
Atividade Prática (AP) da disciplina de Matemática Computacional do curso de Engenharia de Computação da Uninter:

Codificar a mensagem “APROVADO” por criptografia simétrica pelo algoritmo elementar XOR utilizando como chave criptográfica o seu RU ou parte dele. Após a obtenção da cifra decodificá-la comprovando a reciprocidade do processo.

## Instruções
A pasta **"source"** contém o código-fonte do programa criado para resolver o problema proposto. A solução do projeto foi escrita em .NET 4.6 com a linguagem de programção C# utilizando a IDE "Rider" versão 2020.1.3 da Jetbrains, mas a última versão do Visual Studio Community deve conseguir abrir o projeto normalmente. Caso queira somente visualizar o código-fonte basta abrir o arquivo "Program.cs" em um bloco de notas. 

A pasta **"exe"** contém um executável do programa desenvolvido compatível .NET 4.6, o Windows 10 deve suportar essa versão por padrão: https://docs.microsoft.com/en-us/archive/blogs/astebner/mailbag-what-version-of-the-net-framework-is-included-in-what-version-of-the-os

Mas se o seu sistema operacional não suportar o executável você conseguirá facilmente achar na internet uma versão de .NET Framework ou Mono compatível. 

O código é escrito em inglês por uma preferência minha e costume mesmo.

## O Programa

1) O programa pode ser iniciado rodando o arquivo "xor_cryptography.exe" dentro da pasta "exe". Cada etapa do processo ele espera um comando do usuário para prosseguir.

![img-program](/imgs/01.png)

2) A primeira parte é a criptografia, que é iniciada percorrendo cada caracter da palavra "APROVADO" e obtendo seu valor na tabela ASCII em decimal e depois convertendo cada valor decimal para o seu correspondente em binário:

![img-program](/imgs/02.png)

3) Em seguida é obtida a chave de cripografia a partir do RU "3282910". Como o número binário gerado convertendo "3282910" é muito pequeno é feita uma concatenação com cada dígio de "3282910" repetidas vezes até se chegar em uma chave suficientemente grande para cifrar a palavra "APROVADO" toda. A conversão é feita sempre no número resultado de uma vez só e não dígito por digíto, isso permite que a string cifrada gerada seja mais protegida do que em outra abordagens que poderiam utilizar de muitos zeros para a cifragem (como converter dígito a dígito do RU por exemplo).

![img-program](/imgs/03.png)

4) O próximo passo é aplicar o operador XOR entre "APROVADO" em binário e a chave obtida. Note que os primeiros bits da chave não são usados na conversão (a chave obtida no passo anterior tem 59 bits enquanto a palavra "APROVADO" em binário tem 56 bits na conversão utilizada no passo 2.

![img-program](/imgs/04.png)

5) A descriptografia segue os mesmos passos da criptografia, ela só passa a string cifrada ao invés de "APROVADO" para a mesma rotina, utilizando o mesmo RU ("3282910") como base para obtenção da chave. Abaixo a conversão de cada caracter cifrado para binário:

![img-program](/imgs/05.png)

6) Obtenção da chave a partir do número do RU seguindo a mesma lógica anterior:

![img-program](/imgs/06.png)

7) O resultado final usando o operador XOR, provando que é possível obter a string "APROVADO" novamene a partir da string cifrada.
![img-program](/imgs/07.png)
