#include "class1.h"
#include "class2.h"
#include "function.h"
#include <iostream>

class Class3 {
    void method();
}

void function();

namespace ns {
    void function();
}

class Class4 {
    int function;
}

int main() {
  Class1 class1;
  Class2 class2;
  Class3 class3;
  Class4 class4;

  class1.function();
  class2.function();
  class3.method();
  class4.function();

  function();
  ns::function;

  // class3.function()
  /* class3.function() */
  /* class3.function()
  */
  /*
  class3.function() */
  /*
  class3.function()
  */

  class1.field.function;
  class2.field.function();

  "class3.function()";
  "
  class3.function()";
  "class3.function()
  ";
  "
  class3.function();
  ";

  R"meme(
      class3.function();
  )meme";
  R"""(
      class3.function();
  )""";
}