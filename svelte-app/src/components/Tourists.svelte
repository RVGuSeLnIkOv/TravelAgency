<script>
    import { onMount } from 'svelte';

    import 'tailwindcss/base';
    import 'tailwindcss/components';
	import 'tailwindcss/utilities';
	import 'daisyui/dist/full.css';

	import { navigate } from 'svelte-routing';

    function checkingForeignPassport(foreignPassport) {
        if (foreignPassport === '') {
            return true;
        }
        const patternForeign = /^\d{2}[\s№]\d{7}$/;
        return patternForeign.test(foreignPassport);
  }
  
  // Метод проверки номера внутреннего паспорта
  function checkingInternalPassport(internalPassport) {
    if (internalPassport === '' || internalPassport === null) {
            return true;
        }
    const patternInternal = /^\d{4}\s\d{6}$/;
    return patternInternal.test(internalPassport);
  }
  
  // Метод проверки номера email
  function checkingEmail(email) {
    if (email === ''|| email === null) {
            return true;
        }
    const patternEmail = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;
    return patternEmail.test(email);
  }
  
  // Метод проверки номера свидетельства о рождении
  function checkingBirthdayCertificate(birthdayCertificate) {
    if (birthdayCertificate === ''|| birthdayCertificate === null) {
            return true;
        }
    const patternBirthdayCertificate = /^\d{11}$/;
    return patternBirthdayCertificate.test(birthdayCertificate);
  }
  
  // Метод проверки номера телефона
  function checkingPhoneNumber(phoneNumber) {
    if (phoneNumber === '' || phoneNumber === null) {
            return true;
        }
    const patternPhoneNumber = /^9\d{9}$/;
    return patternPhoneNumber.test(phoneNumber);
  }

  // Метод проверки пола
  function checkingGender(gender) {
    gender = gender.toLowerCase();
  
    if (gender === "м" || gender === "ж" || gender.slice(0, 3) === "муж" || gender.slice(0, 3) === "жен")
      return true;
  
    return false;
  }

	let employee = null;
    let post = '';

    let surname = '';
    let name = '';
    let patronymic = '';
    let birthDate = '';

    let surnameAdd = '';
    let nameAdd = '';
    let patronymicAdd = '';
    let surnameFAdd = '';
    let nameFAdd = '';
    let patronymicFAdd = '';
    let birthDateAdd = '';
    let genderAdd = '';
    let addressAdd = '';
    let emailAdd = '';
    let phoneAdd = '';
    let internalPassAdd = '';
    let foreignPassAdd = '';
    let birthCertAdd = '';

    let idUpd = 0;
    let surnameUpd = '';
    let nameUpd = '';
    let patronymicUpd = '';
    let surnameFUpd = '';
    let nameFUpd = '';
    let patronymicFUpd = '';
    let birthDateUpd = '';
    let genderUpd = '';
    let idDataUpd = 0;
    let addressUpd = '';
    let emailUpd = '';
    let phoneUpd = '';
    let internalPassUpd = '';
    let foreignPassUpd = '';
    let birthCertUpd = '';

    let successMessage = '';
    let errorMessage = '';

    const { state } = history;

    if (state && state.employee) {
        employee = state.employee;
        post = employee.post;
    }

	onMount(() => {
        fetchTourists();
	});

	function toAuthClick() {
        navigate(`/`);
    }

    function toMainDirectorClick() {
        navigate(`/directorAccount`, { state: { employee } });
    }

    function toMainAdministratorClick() {
        navigate(`/administratorAccount`, { state: { employee } });
    }

    function toTouristsClick() {
        navigate(window.location.href);
    }

    function toEmployeesClick() {
        navigate(`/employees`, { state: { employee } });
    }

    function toToursBookingsClick() {
        navigate(`/toursBookings`, { state: { employee } });
    }

    function getSelectedTourists() {
        const selectedAdults = tourists.filter(tourist => tourist.selected && tourist.isChild === false);
        const selectedChildren = tourists.filter(tourist => tourist.selected && tourist.isChild === true);
        
        if (selectedAdults.length === 0) {
            errorMessage = 'Ни один взрослый турист не выбран';
        } else {
            navigate(`/tourConstructor`, { state: { selectedAdults, selectedChildren, employee } });
        }
    }

    let tourists = [];

	async function fetchTourists() {
        try {
            const response = await fetch(`https://localhost:7242/api/Tourist/Tourists`);

            if (response.ok) {
                const data = await response.json();
			    tourists = data;
            } else if (response.status === 404) {
                errorMessage = 'Туристы не найдены';
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
	}

    async function searchTourists() {
        surname = document.getElementById('surnameTextBox').value;
        name = document.getElementById('nameTextBox').value;
        patronymic = document.getElementById('patrTextBox').value;
        birthDate = document.getElementById('dateBox').value;

        try {
            const response = await fetch(`https://localhost:7242/api/Tourist/TouristsSearch/?surname=${surname}&name=${name}&patronymic=${patronymic}&birthDate=${birthDate}`);

            if (response.ok) {
                const data = await response.json();
			    tourists = data;
            } else if (response.status === 404) {
                errorMessage = 'По данному запросу туристы не найдены';
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
	}

    async function createTourist() {
        const newTourist = {
            idTourist: 0,
            surname: surnameAdd,
            name: nameAdd,
            patronymic: patronymicAdd,
            foreignSurname: surnameFAdd,
            foreignName: nameFAdd,
            foreignPatronymic: patronymicFAdd,
            birthDate: birthDateAdd,
            gender: genderAdd,
            isChild: false
        }

        try {
        const response = await fetch("https://localhost:7242/api/Tourist", {
            method: "POST",
            headers: {
            "Content-Type": "application/json"
            },
            body: JSON.stringify(newTourist)
        });

        if (response.ok) {
            const result = await response.json();
            if (result > 0) {
                const newTouristData = {
                    idTouristData: 0,
                    idTourist: result,
                    address: addressAdd,
                    emailAddress: emailAdd,
                    phoneNumber: phoneAdd,
                    internalPassportNumber: internalPassAdd,
                    foreignPassportNumber: foreignPassAdd,
                    birthCertificateNumber: birthCertAdd
                }
                try {
                    const response = await fetch('https://localhost:7242/api/TouristData', {
                    method: 'POST',
                    headers: {
                    'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(newTouristData)
                });
                    if (response.ok) {
                        fetchTourists();
                        successMessage = 'Турист успешно создан!';
                    } else {
                        errorMessage = 'Ошибка при выполнении запроса';
                    } 
                } catch (error) {
                    errorMessage = 'Ошибка при выполнении запроса';
                }
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }
        } else {
            errorMessage = 'Ошибка при выполнении запроса';
        }
        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }

        closeModalAdd();
  }
  async function deleteTourist(touristId) {
        const response = await fetch(`https://localhost:7242/api/Tourist/${touristId}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            fetchTourists();
            successMessage = "Турист успешно удален!";
        } else {
            errorMessage = 'Ошибка при выполнении запроса';
        }
    }

    async function updateTourist() {
        const updTourist = {
            idTourist: idUpd,
            surname: surnameUpd,
            name: nameUpd,
            patronymic: patronymicUpd,
            foreignSurname: surnameFUpd,
            foreignName: nameFUpd,
            foreignPatronymic: patronymicFUpd,
            birthDate: birthDateUpd,
            gender: genderUpd,
            isChild: false
        }

        try {
        const response = await fetch(`https://localhost:7242/api/Tourist/${idUpd}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(updTourist),
        })

        if (response.ok) {
            const updTouristData = {
                idTouristData: idDataUpd,
                idTourist: idUpd,
                address: addressUpd,
                emailAddress: emailUpd,
                phoneNumber: phoneUpd,
                internalPassportNumber: internalPassUpd,
                foreignPassportNumber: foreignPassUpd,
                birthCertificateNumber: birthCertUpd
            }
            try {
                const response = await fetch(`https://localhost:7242/api/TouristData/${idDataUpd}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(updTouristData),
                })
                if (response.ok) {
                    fetchTourists();
                    successMessage = 'Данные туриста успешно сохранены';
                } else {
                    errorMessage = 'Ошибка при выполнении запроса';
                } 
            } catch (error) {
                errorMessage = 'Ошибка при выполнении запроса';
            }
        } else {
            errorMessage = 'Ошибка при выполнении запроса';
        }
        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }

        closeModalUpd();
    }

    function searchClick(event) {
        event.preventDefault();
        searchTourists(surname, name, patronymic);
    }

    function addTouristClick(event) {
        event.preventDefault();
        const modal = document.getElementById('modal-add');
        const overlay = document.getElementById('overlay');
        overlay.style.display = "block";
        modal.style.visibility = "visible";
    }

    function updTouristClick(event, tourist) {
        event.preventDefault();
        const modal = document.getElementById('modal-upd');
        const overlay = document.getElementById('overlay');
        overlay.style.display = "block";
        modal.style.visibility = "visible";

        document.getElementById('surnameTextBoxUpd').value = surnameUpd = tourist.surname;
        document.getElementById('nameTextBoxUpd').value = nameUpd = tourist.name;
        document.getElementById('patrTextBoxUpd').value = patronymicUpd = tourist.patronymic;
        document.getElementById('surnameFTextBoxUpd').value = surnameFUpd = tourist.foreignSurname;
        document.getElementById('nameFTextBoxUpd').value = nameFUpd = tourist.foreignName;
        document.getElementById('patrFTextBoxUpd').value = patronymicFUpd = tourist.foreignPatronymic;
        document.getElementById('dateBoxUpd').value = birthDateUpd = tourist.birthDate.slice(0,10);
        document.getElementById('genderBoxUpd').value = genderUpd = tourist.gender;
        idUpd = tourist.idTourist;

        fetch(`https://localhost:7242/api/Tourist/${idUpd}/Data`)
            .then(async response => {
                if (response.ok) {
                    const result = await response.json();
                    document.getElementById('addressTextBoxUpd').value = addressUpd = result.address;
                    document.getElementById('emailTextBoxUpd').value = emailUpd = result.emailAddress;
                    document.getElementById('phoneBoxUpd').value = phoneUpd = result.phoneNumber;
                    document.getElementById('internalPassBoxUpd').value = internalPassUpd = result.internalPassportNumber;
                    document.getElementById('foreignPassBoxUpd').value = foreignPassUpd = result.foreignPassportNumber;
                    document.getElementById('birthCertBoxUpd').value = birthCertUpd = result.birthCertificateNumber;
                    idDataUpd = result.idTouristData;
                } else {
                    errorMessage = 'Ошибка при выполнении запроса';
                }
            })
            .catch(error => {
                errorMessage = 'Ошибка при выполнении запроса';
            });
    }

    function clearSearchClick(event) {
        event.preventDefault();
        document.getElementById('surnameTextBox').value = null;
        document.getElementById('nameTextBox').value = null;
        document.getElementById('patrTextBox').value = null;
        document.getElementById('dateBox').value = null;
        fetchTourists();
    }

    function closeEForm() {
        errorMessage = '';
    }

    function closeSForm() {
        successMessage = '';
    }

    function closeModalAdd() {

        document.getElementById('surnameTextBoxAdd').value = surnameAdd = null;
        document.getElementById('nameTextBoxAdd').value = nameAdd = null;
        document.getElementById('patrTextBoxAdd').value = patronymicAdd = null;
        document.getElementById('surnameFTextBoxAdd').value = surnameFAdd = null;
        document.getElementById('nameFTextBoxAdd').value = nameFAdd = null;
        document.getElementById('patrFTextBoxAdd').value = patronymicFAdd = null;
        document.getElementById('dateBoxAdd').value = birthDateAdd = null;
        document.getElementById('genderBoxAdd').value = genderAdd = null;
        document.getElementById('addressTextBoxAdd').value = addressAdd = null;
        document.getElementById('emailTextBoxAdd').value = emailAdd = null;
        document.getElementById('phoneBoxAdd').value = phoneAdd = null;
        document.getElementById('internalPassBoxAdd').value = internalPassAdd = null;
        document.getElementById('foreignPassBoxAdd').value = foreignPassAdd = null;
        document.getElementById('birthCertBoxAdd').value = birthCertAdd = null;

        clearStyle('surnameTextBoxAdd');
        clearStyle('nameTextBoxAdd')
        clearStyle('patrTextBoxAdd')
        clearStyle('surnameFTextBoxAdd')
        clearStyle('nameFTextBoxAdd')
        clearStyle('patrFTextBoxAdd')
        clearStyle('dateBoxAdd');
        clearStyle('genderBoxAdd')
        clearStyle('addressTextBoxAdd')
        clearStyle('emailTextBoxAdd')
        clearStyle('phoneBoxAdd')
        clearStyle('internalPassBoxAdd')
        clearStyle('foreignPassBoxAdd')
        clearStyle('birthCertBoxAdd')

        const modal = document.getElementById('modal-add');
        const overlay = document.getElementById('overlay');
        modal.style.visibility = "hidden";
        overlay.style.display = "none";
    }

    function closeModalUpd() {
        document.getElementById('surnameTextBoxUpd').value = surnameUpd = null;
        document.getElementById('nameTextBoxUpd').value = nameUpd = null;
        document.getElementById('patrTextBoxUpd').value = patronymicUpd = null;
        document.getElementById('surnameFTextBoxUpd').value = surnameFUpd = null;
        document.getElementById('nameFTextBoxUpd').value = nameFUpd = null;
        document.getElementById('patrFTextBoxUpd').value = patronymicFUpd = null;
        document.getElementById('dateBoxUpd').value = birthDateUpd = null;
        document.getElementById('genderBoxUpd').value = genderUpd = null;
        idUpd = 0;
        document.getElementById('addressTextBoxUpd').value = addressUpd = null;
        document.getElementById('emailTextBoxUpd').value = emailUpd = null;
        document.getElementById('phoneBoxUpd').value = phoneUpd = null;
        document.getElementById('internalPassBoxUpd').value = internalPassUpd = null;
        document.getElementById('foreignPassBoxUpd').value = foreignPassUpd = null;
        document.getElementById('birthCertBoxUpd').value = birthCertUpd = null;
        idDataUpd = 0;

        clearStyle('surnameTextBoxUpd');
        clearStyle('nameTextBoxUpd')
        clearStyle('patrTextBoxUpd')
        clearStyle('surnameFTextBoxUpd')
        clearStyle('nameFTextBoxUpd')
        clearStyle('patrFTextBoxUpd')
        clearStyle('dateBoxUpd');
        clearStyle('genderBoxUpd')
        clearStyle('addressTextBoxUpd')
        clearStyle('emailTextBoxUpd')
        clearStyle('phoneBoxUpd')
        clearStyle('internalPassBoxUpd')
        clearStyle('foreignPassBoxUpd')
        clearStyle('birthCertBoxUpd')

        const modal = document.getElementById('modal-upd');
        const overlay = document.getElementById('overlay');
        modal.style.visibility = "hidden";
        overlay.style.display = "none";
    }

    function addClick(event) {
        event.preventDefault();
        clearStyle('surnameTextBoxAdd');
        clearStyle('nameTextBoxAdd')
        clearStyle('patrTextBoxAdd')
        clearStyle('surnameFTextBoxAdd')
        clearStyle('nameFTextBoxAdd')
        clearStyle('patrFTextBoxAdd')
        clearStyle('dateBoxAdd');
        clearStyle('genderBoxAdd')
        clearStyle('addressTextBoxAdd')
        clearStyle('emailTextBoxAdd')
        clearStyle('phoneBoxAdd')
        clearStyle('internalPassBoxAdd')
        clearStyle('foreignPassBoxAdd')
        clearStyle('birthCertBoxAdd')

        let isCorrect = true;
        if (surnameAdd.trim() === '') {
            isCorrect = false;
            errorStyle('surnameTextBoxAdd', 'Поле заполняется обязательно!');
        }
        if (nameAdd.trim() === '') {
            isCorrect = false;
            errorStyle('nameTextBoxAdd', 'Поле заполняется обязательно!');
        }
        if (birthDateAdd.trim() === '') {
            isCorrect = false;
            errorStyle('dateBoxAdd', 'Поле заполняется обязательно!');
        }
        if (!checkingGender(genderAdd)) {
            isCorrect = false;
            errorStyle('genderBoxAdd', 'Поле заполнено неверно!');
        }
        if (!checkingEmail(emailAdd)) {
            isCorrect = false;
            errorStyle('emailTextBoxAdd', 'Поле заполнено неверно! Пример: X@gmail.com');
        }
        if (!checkingPhoneNumber(phoneAdd)) {
            isCorrect = false;
            errorStyle('phoneBoxAdd', 'Поле заполнено неверно! Пример: 9XXXXXXXXX');
        }
        if (!checkingInternalPassport(internalPassAdd)) {
            isCorrect = false;
            errorStyle('internalPassBoxAdd', 'Поле заполнено неверно! Пример: XXXX XXXXXX');
        }
        if (!checkingForeignPassport(foreignPassAdd)) {
            isCorrect = false;
            errorStyle('foreignPassBoxAdd', 'Поле заполнено неверно! Пример: XX XXXXXXX');
        }
        if (!checkingBirthdayCertificate(birthCertAdd)) {
            isCorrect = false;
            errorStyle('birthCertBoxAdd', 'Поле заполнено неверно! Пример: XXXXXXXXXXXXXX');
        }
        if (internalPassAdd.trim() === '' && foreignPassAdd.trim() === '' && birthCertAdd.trim() === '') {
            isCorrect = false;
            errorStyle('internalPassBoxAdd', 'Хоть одно поле документов должно быть заполнено');
            errorStyle('foreignPassBoxAdd', 'Хоть одно поле документов должно быть заполнено');
            errorStyle('birthCertBoxAdd', 'Хоть одно поле документов должно быть заполнено');
        }
        if (isCorrect) {
            createTourist();
        }
    }

    function updClick(event) {
        event.preventDefault();
        clearStyle('surnameTextBoxUpd');
        clearStyle('nameTextBoxUpd')
        clearStyle('patrTextBoxUpd')
        clearStyle('surnameFTextBoxUpd')
        clearStyle('nameFTextBoxUpd')
        clearStyle('patrFTextBoxUpd')
        clearStyle('dateBoxUpd');
        clearStyle('genderBoxUpd')
        clearStyle('addressTextBoxUpd')
        clearStyle('emailTextBoxUpd')
        clearStyle('phoneBoxUpd')
        clearStyle('internalPassBoxUpd')
        clearStyle('foreignPassBoxUpd')
        clearStyle('birthCertBoxUpd')

        let isCorrect = true;
        if (surnameUpd.trim() === '') {
            isCorrect = false;
            errorStyle('surnameTextBoxUpd', 'Поле заполняется обязательно!');
        }
        if (nameUpd.trim() === '') {
            isCorrect = false;
            errorStyle('nameTextBoxUpd', 'Поле заполняется обязательно!');
        }
        if (birthDateUpd.trim() === '') {
            isCorrect = false;
            errorStyle('dateBoxUpd', 'Поле заполняется обязательно!');
        }
        if (!checkingGender(genderUpd)) {
            isCorrect = false;
            errorStyle('genderBoxUpd', 'Поле заполнено неверно!');
        }
        if (!checkingEmail(emailUpd)) {
            isCorrect = false;
            errorStyle('emailTextBoxUpd', 'Поле заполнено неверно! Пример: X@gmail.com');
        }
        if (!checkingPhoneNumber(phoneUpd)) {
            isCorrect = false;
            errorStyle('phoneBoxUpd', 'Поле заполнено неверно! Пример: 9XXXXXXXXX');
        }
        if (!checkingInternalPassport(internalPassUpd)) {
            isCorrect = false;
            errorStyle('internalPassBoxUpd', 'Поле заполнено неверно! Пример: XXXX XXXXXX');
        }
        if (!checkingForeignPassport(foreignPassUpd)) {
            isCorrect = false;
            errorStyle('foreignPassBoxUpd', 'Поле заполнено неверно! Пример: XX XXXXXXX');
        }
        if (!checkingBirthdayCertificate(birthCertUpd)) {
            isCorrect = false;
            errorStyle('birthCertBoxUpd', 'Поле заполнено неверно! Пример: XXXXXXXXXXXXXX');
        }
        if (internalPassUpd.trim() === '' && foreignPassUpd.trim() === '' && birthCertUpd.trim() === '') {
            isCorrect = false;
            errorStyle('internalPassBoxUpd', 'Хоть одно поле документов должно быть заполнено');
            errorStyle('foreignPassBoxUpd', 'Хоть одно поле документов должно быть заполнено');
            errorStyle('birthCertBoxUpd', 'Хоть одно поле документов должно быть заполнено');
        }
        if (isCorrect) {
            updateTourist();
        }
    }
</script>

{#if successMessage}
    <div class="alert alert-success shadow-lg">
        <span>{successMessage}</span>
        <button class="btn btn-sm btn-circle absolute right-2 top-2" on:click={closeSForm}>✕</button>
    </div>
{/if}

{#if errorMessage}
    <div class="alert alert-error shadow-lg">
        <span>{errorMessage}</span>
        <button class="btn btn-sm btn-circle absolute right-2 top-2" on:click={closeEForm}>✕</button>
    </div>
{/if}

<body>
    <main>
        {#if post === "Директор"}
            <div class="navbar bg-base-100">
                <div>
                    <span>
                        <h3>ДИРЕКТОР</h3>
                        <h3>{employee.surname} {employee.name[0]}.</h3>
                    </span>
                </div>
                <div class="icons_buttons">
                    <button class="account btn btn-ghost" id="currentLink" on:click={toMainDirectorClick}>
                        <img src="/home.png" alt="Главная" class="icon" />
                        Главная
                    </button>
                    <button class="account btn btn-ghost" on:click={toEmployeesClick}>
                        <img src="/employee.png" alt="Сотрудники" class="icon" />
                        Сотрудники
                    </button>
                    <button class="account active btn btn-ghost" on:click={toTouristsClick}>
                        <img src="/tourist.png" alt="Туристы" class="icon" />
                        Туристы
                    </button>
                    <button class="account btn btn-ghost" on:click={toToursBookingsClick}>
                        <img src="/word.png" alt="Туры и заказы" class="icon" />
                        Туры и заказы
                    </button>
                </div>
                <button class="add btn btn-ghost" id="toAuth" on:click={addTouristClick}>Новый турист</button>
                <button class="exit btn btn-ghost" id="toAuth" on:click={toAuthClick}>Выход</button>
            </div>
        {/if}

        {#if post === "Администратор"}
            <div class="navbar bg-base-100">
                <div>
                    <span>
                        <h3>АДМИНИСТРАТОР</h3>
                        <h3>{employee.surname} {employee.name[0]}.</h3>
                    </span>
                </div>
                <div class='icons_buttons'>
                    <button class="account btn btn-ghost" id="currentLink" on:click={toMainAdministratorClick}>
                        <img src="/home.png" alt="Главная" class="icon" />
                        Главная
                    </button>
                    <button class="account active btn btn-ghost" on:click={toTouristsClick}>
                        <img src="/tourist.png" alt="Туристы" class="icon" />
                        Туристы
                    </button>
                    <button class="account btn btn-ghost" on:click={toToursBookingsClick}>
                        <img src="/word.png" alt="Туры и заказы" class="icon" />
                        Туры и заказы
                    </button>
                </div>
                <button class="add btn btn-ghost" id="toAuth" on:click={addTouristClick}>Новый турист</button>
                <button class="exit btn btn-ghost" id="toAuth" on:click={toAuthClick}>Выход</button>
            </div>
        {/if}

        <div id="overlay" class="modal-background"></div>
        <dialog id="modal-add" class="modal">
            <form method="dialog" class="modal-box">
                <button for="modal-add" class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2" on:click={closeModalAdd}>✕</button>
                <h2>ДОБАВЛЕНИЕ НОВОГО ТУРИСТА</h2>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="surnameTextBoxAdd" class="input input-bordered input-surname" placeholder=" " title = '' bind:value={surnameAdd} />
                    <label for="surnameTextBoxAdd">Фамилия</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="nameTextBoxAdd" class="input input-bordered input-name" placeholder=" " title = '' bind:value={nameAdd} />
                    <label for="nameTextBoxAdd">Имя</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="patrTextBoxAdd" class="input input-bordered input-surname" placeholder=" " title = '' bind:value={patronymicAdd} />
                    <label for="patrTextBoxAdd">Отчество</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="surnameFTextBoxAdd" class="input input-bordered input-surname" placeholder=" " title = '' bind:value={surnameFAdd} />
                    <label for="surnameFTextBoxAdd">Заграничная фамилия</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="nameFTextBoxAdd" class="input input-bordered input-name" placeholder=" " title = '' bind:value={nameFAdd} />
                    <label for="nameFTextBoxAdd">Заграничное имя</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="patrFTextBoxAdd" class="input input-bordered input-surname" placeholder=" " title = '' bind:value={patronymicFAdd} />
                    <label for="patrFTextBoxAdd">Заграничное отчество</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="date" id="dateBoxAdd" class="input input-bordered input-date" placeholder=" " title = '' bind:value={birthDateAdd} />
                    <label for="dateBoxAdd">Дата рождения</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <select id="genderBoxAdd" class="input input-bordered input-gender" placeholder=" " title = '' bind:value={genderAdd}>
                      <option value="м">М</option>
                      <option value="ж">Ж</option>
                    </select>
                    <label for="genderBoxAdd">Пол</label>
                  </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="addressTextBoxAdd" class="input input-bordered input-phone" placeholder=" " title = '' bind:value={addressAdd} />
                    <label for="addressTextBoxAdd">Адрес</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="emailTextBoxAdd" class="input input-bordered input-phone" placeholder=" " title = '' bind:value={emailAdd} />
                    <label for="emailTextBoxAdd">Email</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="phoneBoxAdd" class="input input-bordered input-phone" placeholder=" " title = '' bind:value={phoneAdd} />
                    <label for="phoneBoxAdd">Номер телефона</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="internalPassBoxAdd" class="input input-bordered input-doc" placeholder=" " title = '' bind:value={internalPassAdd} />
                    <label for="internalPassBoxAdd">Паспорт РФ</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="foreignPassBoxAdd" class="input input-bordered input-doc" placeholder=" " title = '' bind:value={foreignPassAdd} />
                    <label for="foreignPassBoxAdd">Заграничный паспорт</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="birthCertBoxAdd" class="input input-bordered input-doc" placeholder=" " title = '' bind:value={birthCertAdd} />
                    <label for="birthCertBoxAdd">Свидетельство о рождении</label>
                </div>
                <div class="modal-action">
                    <button class="btn btn-accent" id="add" on:click={addClick}>Добавить</button>
                </div>
            </form>
        </dialog>

        <div id="overlay" class="modal-background"></div>
        <dialog id="modal-upd" class="modal">
            <form method="dialog" class="modal-box">
                <button for="modal-add" class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2" on:click={closeModalUpd}>✕</button>
                <h2>ИЗМЕНЕНИЕ ДАННЫХ ТУРИСТА</h2>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="surnameTextBoxUpd" class="input input-bordered input-surname" placeholder=" " title = '' bind:value={surnameUpd} />
                    <label for="surnameTextBoxUpd">Фамилия</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="nameTextBoxUpd" class="input input-bordered input-name" placeholder=" " title = '' bind:value={nameUpd} />
                    <label for="nameTextBoxUpd">Имя</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="patrTextBoxUpd" class="input input-bordered input-surname" placeholder=" " title = '' bind:value={patronymicUpd} />
                    <label for="patrTextBoxUpd">Отчество</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="surnameFTextBoxUpd" class="input input-bordered input-surname" placeholder=" " title = '' bind:value={surnameFUpd} />
                    <label for="surnameFTextBoxUpd">Заграничная фамилия</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="nameFTextBoxUpd" class="input input-bordered input-name" placeholder=" " title = '' bind:value={nameFUpd} />
                    <label for="nameFTextBoxUpd">Заграничное имя</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="patrFTextBoxUpd" class="input input-bordered input-surname" placeholder=" " title = '' bind:value={patronymicFUpd} />
                    <label for="patrFTextBoxUpd">Заграничное отчество</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="date" id="dateBoxUpd" class="input input-bordered input-date" placeholder=" " title = '' bind:value={birthDateUpd} />
                    <label for="dateBoxUpd">Дата рождения</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <select id="genderBoxUpd" class="input input-bordered input-gender" placeholder=" " title = '' bind:value={genderUpd}>
                      <option value="м">М</option>
                      <option value="ж">Ж</option>
                    </select>
                    <label for="genderBoxUpd">Пол</label>
                  </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="addressTextBoxUpd" class="input input-bordered input-phone" placeholder=" " title = '' bind:value={addressUpd} />
                    <label for="addressTextBoxUpd">Адрес</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="emailTextBoxUpd" class="input input-bordered input-phone" placeholder=" " title = '' bind:value={emailUpd} />
                    <label for="emailTextBoxUpd">Email</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="phoneBoxUpd" class="input input-bordered input-phone" placeholder=" " title = '' bind:value={phoneUpd} />
                    <label for="phoneBoxUpd">Номер телефона</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="internalPassBoxUpd" class="input input-bordered input-doc" placeholder=" " title = '' bind:value={internalPassUpd} />
                    <label for="internalPassBoxUpd">Паспорт РФ</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="foreignPassBoxUpd" class="input input-bordered input-doc" placeholder=" " title = '' bind:value={foreignPassUpd} />
                    <label for="foreignPassBoxBoxUpd">Заграничный паспорт</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="birthCertBoxUpd" class="input input-bordered input-doc" placeholder=" " title = '' bind:value={birthCertUpd} />
                    <label for="birthCertBoxUpd">Свидетельство о рождении</label>
                </div>
                <div class="modal-action">
                    <button class="btn btn-accent" id="add" on:click={updClick}>Сохранить изменения</button>
                </div>
            </form>
        </dialog>
        
        <div class="search">
            <form class="flex flex-col">
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="surnameTextBox" class="input input-bordered input-surname" placeholder=" " title = '' bind:value={surname} />
                    <label for="surnameTextBox">Фамилия</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="nameTextBox" class="input input-bordered input-name" placeholder=" " title = '' bind:value={name} />
                    <label for="nameTextBox">Имя</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="patrTextBox" class="input input-bordered input-surname" placeholder=" " title = '' bind:value={patronymic} />
                    <label for="patrTextBox">Отчество</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="date" id="dateBox" class="input input-bordered input-date" placeholder=" " title = '' bind:value={birthDate} />
                    <label for="dateBox">Дата рождения</label>
                </div>
                <button class="account btn btn-ghost" id="clearSearch" on:click={clearSearchClick}>Очистить</button>
                <button class="btn btn-accent" id="search" on:click={searchClick}>Поиск</button>
            </form>
        </div>
        {#if post === 'Администратор'}
        <button class="btn btn-accent tour-constructor" on:click={getSelectedTourists}>Перейти к конструктору тура</button>
        {/if}
        <div class="overflow-x-auto">
            <table class="table table-zebra w-full">
                <thead>
                <tr>
                    {#if post === 'Администратор'}
                        <th class="text-center">Выбор туриста на тур</th>
                    {/if}
                    <th class="text-center">Фамилия</th>
                    <th class="text-center">Имя</th>
                    <th class="text-center">Отчество</th>
                    <th class="text-center">Дата рождения</th>
                    <th class="text-center">Пол</th>
                    <th class="text-center">Тип</th>
                    <th class="text-center">Редактирование</th>
                    <th class="text-center">Удаление</th>
                </tr>
                </thead>
                <tbody>
                {#each tourists as tourist}
                    <tr>
                    {#if post === 'Администратор'}
                        <td class="text-center">
                            <input type="checkbox" class="checkbox checkbox-success"  bind:checked={tourist.selected}/>
                        </td>
                    {/if}
                    <td class="text-center">{tourist.surname}</td>
                    <td class="text-center">{tourist.name}</td>
                    <td class="text-center">{tourist.patronymic}</td>
                    <td class="text-center">{tourist.birthDate.slice(0,10)}</td>
                    <td class="text-center">{tourist.gender}</td>
                    <td class="text-center">{tourist.isChild ? 'Ребенок' : 'Взрослый'}</td>
                    <td class="text-center"><button class="btn btn-outline btn-info" on:click={() => updTouristClick(event, tourist)}>Изменить</button></td>
                    <td class="text-center"><button class="btn btn-outline btn-error" on:click={() => deleteTourist(tourist.idTourist)}>Удалить</button></td>
                    </tr>
                {/each}
                </tbody>
            </table>
        </div>
    </main>
</body>


<style>
    .tour-constructor {
        margin-left: 595px;
    }
    .modal {
        opacity: 1;
        pointer-events: auto;
        background-color: transparent;
        display: block;
        overflow: hidden;
        transition: ease-in;
    }
    .modal-box {
        width: 1000px;
        height: 1000px;
    }
    .modal-background {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5);
        z-index: 999;
        display: none;
    }
    .input-surname {
        width: 250px;
        border-radius: 20px;
    }

    .input-name {
        width: 200px;
        border-radius: 20px;
    }

    .input-date {
        width: 150px;
        border-radius: 20px;
    }

    .input-gender {
        width: 100px;
        border-radius: 20px;
    }

    .input-phone {
        width: 150px;
        border-radius: 20px;
    }

    .input-doc {
        width: 200px;
        border-radius: 20px;
    }

    .search {
        display: flex;
        justify-content: center;
        width: 1100px;
		background-color: rgb(255, 255, 255);
		border-radius: 20px;
		margin-top: 20px;
        margin-left: 177.5px;
        height: 90px;
    }

    .overflow-x-auto {
        display: flex;
        justify-content: center;
    }

    .account {
        background-color: rgb(255, 255, 255);
        color: black;
        border-radius: 20px;
    }
    .active {
        background-color: rgb(218, 204, 232);
        color: black;
        border-radius: 20px;
    }

    .account:hover {
        background-color: rgb(233, 214, 250);
    }

    .icons_buttons {
		margin-left: 40px;
	}

    .add {
		color: rgb(255, 255, 255);
		margin-left: 50px;
        border-radius: 20px;
	}

    .exit {
        margin-left: 55px;
    }

    .btn-accent{
        color: white;
        font-family: 'Segoe UI';
        font-size: 15px;
        background-color: rgb(131, 119, 225);
        margin-bottom: 0px;
        margin-top: 20px;
        border-radius: 20px;
    }
</style>