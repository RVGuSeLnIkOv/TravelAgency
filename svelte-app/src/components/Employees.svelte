<script>
    import { onMount } from 'svelte';

    import 'tailwindcss/base';
    import 'tailwindcss/components';
	import 'tailwindcss/utilities';
	import 'daisyui/dist/full.css';

	import { navigate } from 'svelte-routing';

    // Метод проверки номера телефона
    function checkingPhoneNumber(phoneNumber) {
        const patternPhoneNumber = /^9\d{9}$/;
        return patternPhoneNumber.test(phoneNumber);
    }
  
    // Метод проверки целого числа
    function checkingInt(input, minValue, maxValue) {
        try {
            input = input.trim();
            let output = parseInt(input, 10);
            if (output < minValue || output > maxValue) {
                return -1;
            }
            if (output === NaN) {
                return -1;
            }
            return output;
        } catch {
            return -1;
        }
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
    let birthDateAdd = '';
    let salaryAdd = '';
    let phoneAdd = '';

    let idUpd = 0;
    let surnameUpd = '';
    let nameUpd = '';
    let patronymicUpd = '';
    let birthDateUpd = '';
    let postUpd = '';
    let salaryUpd = '';
    let phoneUpd = '';

    let successMessage = '';
    let errorMessage = '';

    const { state } = history;

    if (state && state.employee) {
        employee = state.employee;
        post = employee.post;
    }

	onMount(() => {
        fetchEmployees();
	});

	function toAuthClick() {
        navigate(`/`);
    }

    function toMainClick() {
        navigate(`/directorAccount`, { state: { employee } });
    }

    function toTouristsClick() {
        navigate(`/tourists`, { state: { employee } });
    }

    function toEmployeesClick() {
        navigate(window.location.href);
    }

    function toToursBookingsClick() {
        navigate(`/toursBookings`, { state: { employee } });
    }

    let employees = [];

	async function fetchEmployees() {
        try {
            const response = await fetch(`https://localhost:7242/api/Employee/Employees`);

            if (response.ok) {
                const data = await response.json();
			    employees = data;
            } else if (response.status === 404) {
                errorMessage = 'Сотрудники не найдены';
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
	}

    async function searchEmployees() {
        surname = document.getElementById('surnameTextBox').value;
        name = document.getElementById('nameTextBox').value;
        patronymic = document.getElementById('patrTextBox').value;
        birthDate = document.getElementById('dateBox').value;

        try {
            const response = 
                await fetch(`https://localhost:7242/api/Employee/EmployeesSearch/?surname=${surname}&name=${name}&patronymic=${patronymic}&birthDate=${birthDate}`);

            if (response.ok) {
                const data = await response.json();
			    employees = data;
            } else if (response.status === 404) {
                errorMessage = 'По данному запросу сотрудники не найдены';
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
	}

    async function createEmployee() {
        const newEmployee = {
            idEmployee: 0,
            surname: surnameAdd,
            name: nameAdd,
            patronymic: patronymicAdd,
            birthDate: birthDateAdd,
            salary: parseInt(salaryAdd, 10),
            post: "Администратор",
            phoneNumber: phoneAdd
        }
        try {
        const response = await fetch("https://localhost:7242/api/Employee", {
            method: "POST",
            headers: {
            "Content-Type": "application/json"
            },
            body: JSON.stringify(newEmployee)
        });
        if (response.ok) {
            const result = await response.json();
            if (result > 0) {
                const newEmployeeData = {
                    idEmployee: result
                }
                try {
                    const response = await fetch('https://localhost:7242/api/EmployeeData', {
                    method: 'POST',
                    headers: {
                    'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(newEmployeeData)
                });
                    if (response.ok) {
                        fetchEmployees();
                        const login = await response.text();
                        successMessage = `Сотрудник успешно создан! Логин: ${login} Пароль: 1234567890 (необходимо сменить)`;
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
  async function deleteEmployee(employeeId) {
        const response = await fetch(`https://localhost:7242/api/Employee/${employeeId}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            fetchEmployees();
            successMessage = "Сотрудник успешно удален!";
        } else {
            errorMessage = 'Ошибка при выполнении запроса';
        }
    }

    async function updateEmployee() {
        const updEmployee = {
            idEmployee: idUpd,
            surname: surnameUpd,
            name: nameUpd,
            patronymic: patronymicUpd,
            birthDate: birthDateUpd,
            salary: parseInt(salaryUpd, 10),
            post: postUpd,
            phoneNumber: phoneUpd
        }
        fetch(`https://localhost:7242/api/Employee/${idUpd}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(updEmployee),
        })
        .then((response) => {
            if (response.ok) {
                fetchEmployees();
                successMessage = 'Данные сотрудника успешно сохранены';
                if (postUpd === "Директор") {
                    let employeeName = updEmployee.surname + ' ' + updEmployee.name[0] + '.' + updEmployee.patronymic[0] + '.';
                    navigate(`/employees/?employeeName=${employeeName}&post=${postUpd}`);
                }
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }
        })
        .catch((error) => {
            errorMessage = 'Ошибка при выполнении запроса';
        });
        closeModalUpd();
    }

    function searchClick(event) {
        event.preventDefault();
        searchEmployees(surname, name, patronymic);
    }

    function addEmployeeClick(event) {
        event.preventDefault();
        const modal = document.getElementById('modal-add');
        const overlay = document.getElementById('overlay');
        overlay.style.display = "block";
        modal.style.visibility = "visible";
    }

    function updEmployeeClick(event, employee) {
        event.preventDefault();
        const modal = document.getElementById('modal-upd');
        const overlay = document.getElementById('overlay');
        overlay.style.display = "block";
        modal.style.visibility = "visible";

        document.getElementById('surnameTextBoxUpd').value = surnameUpd = employee.surname ;
        document.getElementById('nameTextBoxUpd').value = nameUpd = employee.name;
        document.getElementById('patrTextBoxUpd').value = patronymicUpd = employee.patronymic;
        document.getElementById('dateBoxUpd').value = birthDateUpd = employee.birthDate.slice(0,10);
        document.getElementById('salaryBoxUpd').value = salaryUpd = employee.salary;
        document.getElementById('phoneBoxUpd').value = phoneUpd = employee.phoneNumber;
        idUpd = employee.idEmployee;
        postUpd = employee.post;
    }

    function clearSearchClick(event) {
        event.preventDefault();
        document.getElementById('surnameTextBox').value = null;
        document.getElementById('nameTextBox').value = null;
        document.getElementById('patrTextBox').value = null;
        document.getElementById('dateBox').value = null;
        fetchEmployees();
    }

    function closeEForm() {
        errorMessage = '';
    }

    function closeSForm() {
        successMessage = '';
    }

    function closeModalAdd() {
        document.getElementById('surnameTextBoxAdd').value = surnameAdd = null ;
        document.getElementById('nameTextBoxAdd').value = nameAdd = null;
        document.getElementById('patrTextBoxAdd').value = patronymicAdd = null;
        document.getElementById('dateBoxAdd').value = birthDateAdd = null;
        document.getElementById('salaryBoxAdd').value = salaryAdd = null;
        document.getElementById('phoneBoxAdd').value = phoneAdd = null;

        clearStyle('surnameTextBoxAdd');
        clearStyle('nameTextBoxAdd')
        clearStyle('patrTextBoxAdd')
        clearStyle('dateBoxAdd')
        clearStyle('salaryBoxAdd')
        clearStyle('phoneBoxAdd')

        const modal = document.getElementById('modal-add');
        const overlay = document.getElementById('overlay');
        modal.style.visibility = "hidden";
        overlay.style.display = "none";
    }

    function closeModalUpd() {
        document.getElementById('surnameTextBoxUpd').value = surnameUpd = null ;
        document.getElementById('nameTextBoxUpd').value = nameUpd = null;
        document.getElementById('patrTextBoxUpd').value = patronymicUpd = null;
        document.getElementById('dateBoxUpd').value = birthDateUpd = null;
        document.getElementById('salaryBoxUpd').value = salaryUpd = null;
        document.getElementById('phoneBoxUpd').value = phoneUpd = null;
        idUpd = 0;
        postUpd = null;

        clearStyle('surnameTextBoxUpd');
        clearStyle('nameTextBoxUpd')
        clearStyle('patrTextBoxUpd')
        clearStyle('dateBoxUpd')
        clearStyle('salaryBoxUpd')
        clearStyle('phoneBoxUpd')

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
        clearStyle('dateBoxAdd')
        clearStyle('salaryBoxAdd')
        clearStyle('phoneBoxAdd')

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
        if (isNaN(checkingInt(salaryAdd, 0, 1000000))) {
            isCorrect = false;
            errorStyle('salaryBoxAdd', 'Поле заполнено неверно!');
        }
        if (!checkingPhoneNumber(phoneAdd)) {
            isCorrect = false;
            errorStyle('phoneBoxAdd', 'Поле заполнено неверно! Пример: 9XXXXXXXXX');
        }
        if (isCorrect) {
            createEmployee();
        }
    }

    function updClick(event) {
        event.preventDefault();
        clearStyle('surnameTextBoxUpd');
        clearStyle('nameTextBoxUpd')
        clearStyle('patrTextBoxUpd')
        clearStyle('dateBoxUpd')
        clearStyle('salaryBoxUpd')
        clearStyle('phoneBoxUpd')

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
        if (isNaN(checkingInt(salaryUpd, 0, 1000000))) {
            isCorrect = false;
            errorStyle('salaryBoxUpd', 'Поле заполнено неверно!');
        }
        if (!checkingPhoneNumber(phoneUpd)) {
            isCorrect = false;
            errorStyle('phoneBoxUpd', 'Поле заполнено неверно! Пример: 9XXXXXXXXX');
        }
        if (isCorrect) {
            updateEmployee();
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
        <div class="navbar bg-base-100">
            <div>
                <span>
                    <h3>ДИРЕКТОР</h3>
                    <h3>{employee.surname} {employee.name[0]}.</h3>
                </span>
            </div>
            <div class="icons_buttons">
                <button class="account btn btn-ghost" id="currentLink" on:click={toMainClick}>
                    <img src="/home.png" alt="Главная" class="icon" />
                    Главная
                </button>
                <button class="account active btn btn-ghost" on:click={toEmployeesClick}>
                    <img src="/employee.png" alt="Сотрудники" class="icon" />
                    Сотрудники
                </button>
                <button class="account btn btn-ghost" on:click={toTouristsClick}>
                    <img src="/tourist.png" alt="Туристы" class="icon" />
                    Туристы
                </button>
                <button class="account btn btn-ghost" on:click={toToursBookingsClick}>
                    <img src="/word.png" alt="Туры и заказы" class="icon" />
                    Туры и заказы
                </button>
            </div>
            <button class="add btn btn-ghost" id="addEmployee" type="button" on:click={addEmployeeClick}>Новый сотрудник</button>
            <button class="exit btn btn-ghost" id="toAuth" on:click={toAuthClick}>Выход</button>
        </div>

        <div id="overlay" class="modal-background"></div>
        <dialog id="modal-add" class="modal">
            <form method="dialog" class="modal-box">
                <button for="modal-add" class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2" on:click={closeModalAdd}>✕</button>
                <h2>ДОБАВЛЕНИЕ НОВОГО СОТРУДНИКА</h2>
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
                    <input type="date" id="dateBoxAdd" class="input input-bordered input-date" placeholder=" " title = '' bind:value={birthDateAdd} />
                    <label for="dateBoxAdd">Дата рождения</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="salaryBoxAdd" class="input input-bordered input-salary" placeholder=" " title = '' bind:value={salaryAdd} />
                    <label for="salaryBoxAdd">Зарплата</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="phoneBoxAdd" class="input input-bordered input-phone" placeholder=" " title = '' bind:value={phoneAdd} />
                    <label for="phoneBoxAdd">Номер телефона</label>
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
                <h2>ИЗМЕНЕНИЕ ДАННЫХ СОТРУДНИКА</h2>
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
                    <input type="date" id="dateBoxUpd" class="input input-bordered input-date" placeholder=" " title = '' bind:value={birthDateUpd} />
                    <label for="dateBoxUpd">Дата рождения</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="salaryBoxUpd" class="input input-bordered input-salary" placeholder=" " title = '' bind:value={salaryUpd} />
                    <label for="salaryBoxUpd">Зарплата</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="phoneBoxUpd" class="input input-bordered input-phone" placeholder=" " title = '' bind:value={phoneUpd} />
                    <label for="phoneBoxUpd">Номер телефона</label>
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
        <div class="overflow-x-auto">
            <table class="table table-zebra w-full">
                <thead>
                <tr>
                    <th class="text-center">Фамилия</th>
                    <th class="text-center">Имя</th>
                    <th class="text-center">Отчество</th>
                    <th class="text-center">Дата рождения</th>
                    <th class="text-center">Зарплата</th>
                    <th class="text-center">Должность</th>
                    <th class="text-center">Номер телефона</th>
                    <th class="text-center">Редактирование</th>
                    <th class="text-center">Удаление</th>
                </tr>
                </thead>
                <tbody>
                {#each employees as employee}
                    <tr>
                    <td class="text-center">{employee.surname}</td>
                    <td class="text-center">{employee.name}</td>
                    <td class="text-center">{employee.patronymic}</td>
                    <td class="text-center">{employee.birthDate.slice(0,10)}</td>
                    <td class="text-center">{employee.salary}</td>
                    <td class="text-center">{employee.post}</td>
                    <td class="text-center">{employee.phoneNumber}</td>
                    <td class="text-center"><button class="btn btn-outline btn-info" on:click={() => updEmployeeClick(event, employee)}>Изменить</button></td>
                    {#if employee.post.toLowerCase() !== 'директор'}
                        <td class="text-center"><button class="btn btn-outline btn-error" on:click={() => deleteEmployee(employee.idEmployee)}>Удалить</button></td>
                    {:else}
                        <td></td>
                    {/if}
                    </tr>
                {/each}
                </tbody>
            </table>
        </div>
    </main>
</body>


<style>
    .modal {
        opacity: 1;
        pointer-events: auto;
        background-color: transparent;
        display: block;
        overflow: hidden;
        transition: ease-in;
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

    .input-salary {
        width: 150px;
        border-radius: 20px;
    }

    .input-phone {
        width: 150px;
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
		margin-left: 40px;
        border-radius: 20px;
	}

    .exit {
        margin-left: 35px;
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