#include <stdio.h>
#include <string.h>
#include <stdbool.h>
#include <locale.h>
#include <wchar.h>
#include <wctype.h>

#define MAX 100
#define N 31
#define MIN 7

wchar_t LETTERS[] = L"AĂÂBCDEFGHIÎJKLMNOPQRSȘTȚUVWXYZ";  
wchar_t LETTERS_SMALL[] = L"aăâbcdefghiîjklmnopqrsștțuvwxyz";
wchar_t changedAlphabet[N + 1];

int LetterCode(wchar_t c, wchar_t alphabet[])
{
    for(int i = 0; i < N; i++)
    {
        if(c == alphabet[i])
        {
            return i;
        }
    }

    return -1;
}

void ToUpperMessage(wchar_t *mes)
{
    for(int i = 0; mes[i] != L'\0'; i++)
    {
        int code = LetterCode(mes[i], LETTERS);

        if(code == -1)
            code = LetterCode(mes[i], LETTERS_SMALL);

        if(code != -1)
            mes[i] = LETTERS[code];
    }
}

void ClearBuffer()
{
    wint_t c;
    while((c = fgetwc(stdin)) != L'\n' && c != WEOF);
}

int ReadIntegerKey()
{
    int key;
    wprintf(L"Introduceti cheia (1 - 30): ");
    if(wscanf(L"%d", &key) != 1) key = 0;
    ClearBuffer();
    return key;
}

bool CheckIntegerKey(int key)
{
    if (key < 1 || key > 30)
    {
        wprintf(L"Cheie gresita! Valorile permise sunt de la 1 la 30.\n");
        return false;
    }
    return true;
}


void ReadMessage(wchar_t *mes)
{
    wprintf(L"Introduceti mesajul cu litere din alfabetul roman: ");
    fgetws(mes, MAX, stdin);
    mes[wcscspn(mes, L"\n")] = L'\0';
    
    int j = 0;
    for(int i = 0; mes[i] != L'\0'; i++)
    {
        if(!iswspace(mes[i]))
        {
            mes[j++] = mes[i];
        }
    }
    mes[j] = L'\0';

    ToUpperMessage(mes);
}

bool CheckMessage(wchar_t *mes)
{
    if(mes[0] == L'\0')
    {
        wprintf(L"Mesajul nu poate fi gol\n");
        return false;
    }
    for(int i = 0; mes[i] != L'\0'; i++)
    {
        if(LetterCode(mes[i], LETTERS) == -1)
        {
            wprintf(L"Caracterul '%lc' nu este permis!\n", mes[i]);
            return false;
        }
    }
    return true;
}

void ChangeOneKey(wchar_t *mes, int key)
{     
    for(int i = 0; mes[i] != L'\0'; i++)
    {
        int pos = LetterCode(mes[i], LETTERS);        
        int j = (pos + key) % N;      
        if(j < 0) j += N;
        mes[i] = LETTERS[j];
    }
}

void EncryptOneKey()
{
    wchar_t message[MAX];
    int key = ReadIntegerKey();
    if(!CheckIntegerKey(key)) return;

    ReadMessage(message);    
    if(!CheckMessage(message)) return;

    ChangeOneKey(message, key);
    wprintf(L"Mesajul criptat: %ls\n", message);
}

void DecryptOneKey()
{
    wchar_t message[MAX];
    int key = ReadIntegerKey();
    if(!CheckIntegerKey(key)) return;

    ReadMessage(message);
    if(!CheckMessage(message)) return;

    ChangeOneKey(message, -key);
    wprintf(L"Mesajul decriptat: %ls\n", message);
}


void OneKey()
{
    int opt = 0;
    wprintf(L"Criptare - 1 \n");
    wprintf(L"Decriptare - 2 \n");
    wprintf(L"Alegeti optiunea: ");
    if(wscanf(L"%d", &opt) != 1) opt = 0;
    ClearBuffer();

    switch(opt)
    {
        case 1:
            EncryptOneKey();
            break;
        case 2:
            DecryptOneKey();
            break;
        default:
            wprintf(L"Optiune gresita!");
            break;
    }

}

void ReadStringKey(wchar_t *key2)
{
    wprintf(L"Introduceti cheia din minim %d litere: ", MIN);
    fgetws(key2, MAX, stdin);
    key2[wcscspn(key2, L"\n")] = L'\0';

    int j = 0;
    for(int i = 0; key2[i] != L'\0'; i++)
    {
        if(key2[i] != L' ')
        {
            key2[j++] = key2[i];
        }
    }
    key2[j] = L'\0';

    ToUpperMessage(key2);
}

bool CheckStringKey(wchar_t *key2)
{
    if(wcslen(key2) < MIN)
    {
        wprintf(L"Cheia trebuie sa aiba minim %d litere\n", MIN);
        return false;
    }

    for(int i = 0; key2[i] != L'\0'; i++)
    {
        if(LetterCode(key2[i], LETTERS) == -1)
        {
            wprintf(L"Caracterul '%lc' nu este permis in cheia 2!\n", key2[i]);
            wprintf(L"Cheia 2 poate contine doar litere din alfabetul roman.\n");
            return false;
        }
    }
    return true;
}

void ChangeTwoKeys(wchar_t *mes, int key1, wchar_t *key2)
{
    memset(changedAlphabet, 0, sizeof(changedAlphabet));
    int pos = 0;
    for(int i = 0; key2[i] != L'\0'; i++)
    {
        if(LetterCode(key2[i], changedAlphabet) == -1)
        {
            changedAlphabet[pos++] = key2[i];
        }
    }

    for(int i = 0; i < N; i++)
    {
        if(LetterCode(LETTERS[i], changedAlphabet) == -1)
        {        
            changedAlphabet[pos++] = LETTERS[i];
        }
    }
    changedAlphabet[pos] = L'\0';

    for(int i = 0; mes[i] != L'\0'; i++)
    {
        int elem = LetterCode(mes[i], changedAlphabet);

        int newPos = (elem + key1) % N;

        if(newPos < 0) newPos += N;

        mes[i] = changedAlphabet[newPos];
    }

}

void EncryptTwoKeys()
{
    wchar_t message[MAX];
    wchar_t stringKey[MAX];
    int intKey = ReadIntegerKey();
    if(!CheckIntegerKey(intKey)) return;

    ReadStringKey(stringKey);
    if(!CheckStringKey(stringKey)) return;

    ReadMessage(message);
    if(!CheckMessage(message)) return;

    ChangeTwoKeys(message, intKey, stringKey);
    wprintf(L"Mesajul criptat este: %ls\n", message);
}

void DecryptTwoKeys()
{
    wchar_t message[MAX];
    wchar_t stringKey[MAX];
    int intKey = ReadIntegerKey();
    if(!CheckIntegerKey(intKey)) return;

    ReadStringKey(stringKey);
    if(!CheckStringKey(stringKey)) return;

    ReadMessage(message);
    if(!CheckMessage(message)) return;

    ChangeTwoKeys(message, -intKey, stringKey);
    wprintf(L"Mesajul decriptat este: %ls\n", message);
}

void TwoKeys()
{
    int opt = 0;
    wprintf(L"Criptare - 1 \n");
    wprintf(L"Decriptare - 2 \n");
    wprintf(L"Alegeti optiunea: ");
    if(wscanf(L"%d", &opt) != 1) opt = 0;
    ClearBuffer();

    switch(opt)
    {
        case 1:
            EncryptTwoKeys();
            break;
        case 2:
            DecryptTwoKeys();
            break;
        default:
            wprintf(L"Optiune gresita!");
            break;
    }
}

int main()
{
    setlocale(LC_ALL, "");
    while(true)
    {
        int opt = 0;
        wprintf(L"Criptare cu o cheie - 1 \n");
        wprintf(L"Criptare cu doua chei - 2 \n");
        wprintf(L"Iesi din program - 3 \n");
        wprintf(L"Alege optiunea: ");

        int rez = wscanf(L"%d", &opt);
        if(rez == EOF) return 0;   
        if(rez != 1) opt = 0;
        ClearBuffer();

        switch (opt)
        {
            case 1:
                OneKey();
                break;
            case 2:
                TwoKeys();
                break;
            case 3: 
                return 0;
                break;
            default:
                wprintf(L"Optiune gresita");
                break;
        }
    }
    return 0;
}