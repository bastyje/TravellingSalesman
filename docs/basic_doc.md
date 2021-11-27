# Genetic Algorithm

## Ogólne za³o¿enia

Program Genetic Algorithm implementuje algorytm genetyczny, który umo¿liwia znalezienie maksimum funkcji dopasowania jednej zmiennej w zadanej dziedzinie liczb ca³kowitych.

### Genetic Algorithm implementuje takie operacje jak:
- reprodukcja z u¿yciem nieproporcjonalnej ruletki
- krzy¿owanie proste
- mutacja równomierna

### U¿ytkownik ma wp³yw na:
- wzór przekazanej funkcji
- rozmiar populacji
- parametry krzy¿owania
- parametry mutacji

### Wynik dzia³ania programu jest na bie¿¹co wizualizowany za pomoc¹ wykresów:
- œredniego przystosowania
- maksymalnego przystosowania
- minimalnego przystosowania
- wykresu funkcji w zadanym wczeœniej przedziale

## Reprodukcja

1. Na podstwie funkcji przystosowania obliczane jest przystosowanie ka¿dego osobnika. *Prawdopodobieñstwo wyboru* danego osobnika do reprodukcji obliczane jest poprzezpodzielenie wartoœci funkcji przystosowania przez sumê wartoœci funkcji przystosowania wszystkich elementów populacji.
2. Ze starej populacji wybierane s¹ elementy nowej populacji w liczbie równej cz³onkom poprzedniej populacji. Element wybierany jest z prawdopodobieñstwem równym *prawdopodobieñstwu wyboru*.
3. Elementy wybrane zapisywane s¹ jako populacja tymczasowa.
4. Operacja reprodukcji siê koñczy.

## Krzy¿owanie

1. Z otrzymanej populacji wybierane s¹ w sposób losowy pary osobników.
2. Krzy¿owanie jest przeprowadzane z prawdopodobieñstwem równym zadanemu prawdopodobieñstwu krzy¿owania.
3. Dla ci¹gu kodowego o d³ugoœci l, w sposób losowy z jednostajnym rozk³adem prawdopodobieñstwa wybierana jest liczba *i* z zakresu 1-(l - 1), która reprezentuje pierwszy indeks ci¹gu kodowego, który podlega operacji krzy¿owania.
4. Elementy ci¹gu kodowego z zakresu *i*-(l - 1) zamieniaj¹ siê pomiêdzy sparowanymi ci¹gami kodowymi.
5. Operacja krzy¿owania siê koñczy.

## Mutacja

1. Operacja jest przeprowadzana na ka¿dym po kolei ci¹gu kodowym (chromosomie) z populacji.
2. Nastêpuje iteracja po ka¿dym allelu chromosomu.
3. Dla ka¿dego allela operacja mutacji jest przeprowadzana z prawdopodobieñstwem równym zadanemu prawdopodobieñstwu mutacji.
4. Wartoœæ allela zmieniana jest na przeciwn¹.
5. Operacja mutacji siê koñczy