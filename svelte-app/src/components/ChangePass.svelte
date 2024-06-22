<script>
    import { navigate } from 'svelte-routing';
    import 'tailwindcss/base';
    import 'tailwindcss/components';
	import 'tailwindcss/utilities';
	import 'daisyui/dist/full.css';

    let id = -1;
    let idEmployee = -1;
    let login = '';
    let password = '';
    let passwordRep = '';

    let successMessage = '';
    let errorMessage = '';

    async function changePass(login, password) {
        clearStyle('loginTextBox');
        clearStyle('passBox');
        clearStyle('passBoxRep');
        
        try {
            const response = await fetch(`https://localhost:7242/api/EmployeeData/${login}`);

            if (!response.ok) {
                errorMessage = 'Ошибка при выполнении запроса';
            } else {
                let result = await response.json();
                id = result.idEmployeeData;
                idEmployee = result.idEmployee;
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }

        const updateEmployeeData = {
            idEmployeeData: id,
            idEmployee: idEmployee,
            login: login,
            password: password
        };

        fetch(`https://localhost:7242/api/EmployeeData/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updateEmployeeData)
        })
        .then(response => {
            if (response.ok) {
                successMessage = 'Пароль успешно обновлен';
                setTimeout(() => {
                    navigate('/');
                }, 2000);
            } else if (response.status === 404) {
                errorMessage = 'Данные не найдены';
            } else if (response.status === 422) {
                errorMessage = 'Некорректные данные';
            } else {
                console.log(error);
                errorMessage = 'Ошибка при выполнении запроса';
            }
        })
        .catch(error => {
            errorMessage = 'Ошибка при выполнении запроса';
        });
    }
  
  // Обработчик события отправки формы
    function changePassClick(event) {
        clearStyle('loginTextBox');
        clearStyle('passBox');
        clearStyle('passBoxRep');

        event.preventDefault()
        const login = document.getElementById('loginTextBox').value;
        const password = document.getElementById('passBox').value;
        const passwordRep = document.getElementById('passBoxRep').value;

        if (login.trim() === '' || password.trim() === '' || passwordRep.trim() === '') {
            errorStyle('loginTextBox', 'Данные введены не полностью или вообще не введены!');
            errorStyle('passBox', 'Данные введены не полностью или вообще не введены!');
            errorStyle('passBoxRep', 'Данные введены не полностью или вообще не введены!');
        } else if (password !== passwordRep) {
            clearStyle('loginTextBox');
            errorStyle('passBox', 'Пароли не совпадают!');
            errorStyle('passBoxRep', 'Пароли не совпадают!');
        } else if (password.length < 9) {
            clearStyle('loginTextBox');
            errorStyle('passBox', 'Пароль не может быть меньше 9 символов!');
            errorStyle('passBoxRep', 'Пароль не может быть меньше 9 символов!');
        } else {
            changePass(login, password);
        }
    }

    function toAuthClick() {
        navigate('/')
    }

    function closeForm() {
        errorMessage = '';
    }
</script>

{#if successMessage}
    <div class="alert alert-success shadow-lg">
        <span>{successMessage}</span>
    </div>
{/if}

{#if errorMessage}
    <div class="alert alert-error shadow-lg">
        <span>{errorMessage}</span>
        <button class="btn btn-sm btn-circle absolute right-2 top-2" on:click={closeForm}>✕</button>
    </div>
{/if}

<body>
    <main>
        <div class="border">
            <form class="flex flex-col">
                <h1>Туристическое агентство</h1>
                <h2>ИЗМЕНЕНИЕ ПАРОЛЯ</h2>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="loginTextBox" class="input input-bordered" placeholder=" " title = '' bind:value={login} required />
                    <label for="loginTextBox">Логин</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="password" id="passBox" placeholder=" " class="input input-bordered" title = '' bind:value={password} required />
                    <label for="passBox">Новый пароль</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="password" id="passBoxRep" placeholder=" " class="input input-bordered" title = '' bind:value={passwordRep} required />
                    <label for="passBoxRep">Повторение нового пароля</label>
                </div>
                <button class="btn btn-accent" type="changePass" on:click={changePassClick}>Сменить пароль</button>
                <button class="btn btn-ghost" type="toAuth" on:click={toAuthClick}>Вернуться к авторизации</button>
            </form>
        </div>
    </main>
</body>
  
<style>
    main {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 100vh;
    }

    .btn-ghost {
        font-family: 'Segoe UI';
        font-size: 10px;
        font-weight: thin;
        color: rgb(103, 58, 183);
        margin-bottom: 20px;
        margin-left: 0px;
        font-style: italic;
        margin-right: 260px;
        background-color: transparent;
    }

    .btn-accent{
        color: white;
        font-family: 'Segoe UI';
        font-size: 15px;
        background-color: rgb(103, 58, 183);
        margin-bottom: 0px;
        margin-top: 20px;
        width: 400px;
    }

    .btn:active {
        background-color: transparent;
    }

    .border {
        display: flex;
        justify-content: center;
        align-items: center;
        text-align: center;
        width: 550px;
        border-radius: 20px;
        background-color: rgb(165, 211, 248);
        position: absolute;
    }
</style>