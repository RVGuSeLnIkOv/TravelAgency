<script>
    import { onMount } from 'svelte';

    import 'tailwindcss/base';
    import 'tailwindcss/components';
	import 'tailwindcss/utilities';
	import 'daisyui/dist/full.css';

	import { navigate } from 'svelte-routing';

	let employee = null;
	let post = '';

	const { state } = history;

	if (state && state.employee) {
		employee = state.employee;
		post = employee.post;
	}

	onMount(() => {
		var currentURL = window.location.href;
  		var linkElement = document.getElementById('currentLink');
  		linkElement.href = currentURL;
	});

	function toAuthClick() {
        navigate(`/`);
    }

    function toMainClick() {
        navigate(window.location.href);
    }

    function toTouristsClick() {
        navigate(`/tourists`, { state: { employee } });
    }

    function toToursBookingsClick() {
        navigate(`/toursBookings`, { state: { employee } });
    }
    
</script>
	
<body>
	<main>
		<div class="navbar bg-base-100">
			<div>
				<span>
					<h3>АДМИНИСТРАТОР</h3>
					<h3>{employee.surname} {employee.name[0]}.</h3>
				</span>
			</div>
			<div class='icons_buttons'>
                <button class="account active btn btn-ghost" id="currentLink" on:click={toMainClick}>
                    <img src="/home.png" alt="Главная" class="icon" />
                    Главная
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
            <button class="exit btn btn-ghost" id="toAuth" on:click={toAuthClick}>Выход</button>
		</div>
		<div>
			<img class="air" src="/air.jpg" alt="">
		</div>
		<div>
			<img class="city" src="/city.png" alt="">
		</div>
		<div>
			<img class="island" src="/island.jpg" alt="">
		</div>
	</main>
</body>

<style>
    .account {
        background-color: rgb(255, 255, 255);
        color: black;
        z-index: 2;
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

	.exit {
		margin-left: 250px;
	}
	.air {
		width: 500px;
		left: 940px;
		top: 100px;
		border-radius: 20px;
        position: fixed;
		z-index: 1;
	}
	.city {
		width: 500px;
		left: 40px;
		top: 100px;
		border-radius: 20px;
        position: fixed;
		z-index: 1;
	}
	.island {
		width: 352px;
		left: 564px;
		top: 100px;
		border-radius: 20px;
        position: fixed;
		z-index: 1;
	}
</style>