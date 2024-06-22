function checkingForeignPassport(foreignPassport) {
    const patternForeign = /^\d{2}[\s№]\d{7}$/;
    return patternForeign.test(foreignPassport);
  }
  
  // Метод проверки номера внутреннего паспорта
  function checkingInternalPassport(internalPassport) {
    const patternInternal = /^\d{4}\s\d{6}$/;
    return patternInternal.test(internalPassport);
  }
  
  // Метод проверки номера email
  function checkingEmail(email) {
    const patternEmail = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;
    return patternEmail.test(email);
  }
  
  // Метод проверки номера свидетельства о рождении
  function checkingBirthdayCertificate(birthdayCertificate) {
    const patternBirthdayCertificate = /^\d{14}$/;
    return patternBirthdayCertificate.test(birthdayCertificate);
  }
  
  // Метод проверки номера телефона
  function checkingPhoneNumber(phoneNumber) {
    const patternPhoneNumber = /^9\d{9}$/;
    return patternPhoneNumber.test(phoneNumber);
  }

  // Метод проверки пола
  function checkingGender(gender) {
    gender = gender.trim().toLowerCase();
  
    if (gender === "м" || gender === "ж" || gender.slice(0, 3) === "муж" || gender.slice(0, 3) === "жен")
      return true;
  
    return false;
  }
  
  // Метод проверки должности
  function checkingPost(post) {
    post = post.trim().toLowerCase();
  
    if (post === "директор" || post === "администратор")
      return true;
  
    return false;
  }
  
  // Метод проверки целого числа
  function checkingInt(input, minValue, maxValue) {
    try {
      input = input.trim();
      let output = parseInt(input, 10);
      if (output < minValue || output > maxValue)
        return -1;
      return output;
    } catch {
      return -1;
    }
  }