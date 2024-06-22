<script>
    import { navigate } from 'svelte-routing';
    import 'tailwindcss/base';
    import 'tailwindcss/components';
	import 'tailwindcss/utilities';
	import 'daisyui/dist/full.css';

    let idEmployee = '';
    let employee = null;

    let login = '';
    let password = '';

    let successMessage = '';
    let errorMessage = '';

    async function auth(login, password) {
        clearStyle('loginTextBox');
        clearStyle('passBox')

        try {
            const response = await fetch(`https://localhost:7242/api/EmployeeData/Auth/${login}/${password}`);

            if (response.ok) {
            const data = await response.json();
                if (data > 0) {
                    idEmployee = data;
                    try {
                        const response = await fetch(`https://localhost:7242/api/Employee/${idEmployee}`);

                        if (response.ok) {
                            employee = await response.json();
                        } else if (response.status === 404) {
                            errorMessage = 'Сотрудник не найден';
                        } else {
                            errorMessage = 'Ошибка при выполнении запроса';
                        }

                    } catch (error) {
                        errorMessage = 'Ошибка при выполнении запроса';
                    }

                    successMessage = 'Выполняется вход...';
                    if (employee.post === 'Директор') {
                        setTimeout(() => {
                        navigate(`/directorAccount`, { state: { employee } });
                    }   , 2000);
                    }
                    else {
                        setTimeout(() => {
                        navigate(`/administratorAccount`, { state: { employee } });
                    }   , 2000);
                    }
                } else {
                    errorStyle('loginTextBox', 'Неверный логин или пароль!');
                    errorStyle('passBox', 'Неверный логин или пароль!');
                }
            } else {
                errorMessage = 'Ошибка при выполнении запроса';
            }

        } catch (error) {
            errorMessage = 'Ошибка при выполнении запроса';
        }
    }
  
  // Обработчик события отправки формы
    function enterClick(event) {
        clearStyle('loginTextBox');
        clearStyle('passBox')

        event.preventDefault()
        const login = document.getElementById('loginTextBox').value;
        const password = document.getElementById('passBox').value;

        if (login.trim() !== '' && password.trim() !== '' && password.length > 8) {
            auth(login, password);
        } else {
            errorStyle('loginTextBox', 'Ошибка в логине или пароле!');
            errorStyle('passBox', 'Ошибка в логине или пароле!');
        }
    }

    function toChangePassClick() {
        navigate(`/changePass`);
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
                <h2>АВТОРИЗАЦИЯ</h2>
                <div class="form-control w-full max-w-xs">
                    <input type="text" id="loginTextBox" class="input input-bordered" placeholder=" " title = '' bind:value={login} required />
                    <label for="loginTextBox">Логин</label>
                </div>
                <div class="form-control w-full max-w-xs">
                    <input type="password" id="passBox" placeholder=" " class="input input-bordered" title = '' bind:value={password} required />
                    <label for="passBox">Пароль</label>
                </div>
                <button class="btn btn-accent" type="enter" on:click={enterClick}>Войти</button>
                <button class="btn btn-ghost" type="changePass" on:click={toChangePassClick}>Забыли пароль?</button>
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
        margin-right: 310px;
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
  
  
  
  
  
  