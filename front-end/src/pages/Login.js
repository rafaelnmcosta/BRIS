import React, { useState } from 'react';
import axios from 'axios';

import '../App.css';
import './Login.css';
import logoUfg from "../assets/UFG_logo.png";
import logoEvz from "../assets/EVZ_UFG.svg";
import logoInf from "../assets/INF_UFG.svg";

import FormLogin from '../components/FormLogin';
import HeadbarLogin from '../components/HeadbarLogin';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async () => {
      try {
          const response = await axios.post('https://yourapi.com/api/auth/login', {
              username,
              password
          });
          const { token } = response.data;
          localStorage.setItem('token', token);
      } catch (error) {
          console.error('Login failed', error);
      }
  };

  return (
    <div>
      <HeadbarLogin/>
      <div className='main-content'> 
        <div className='text-section'>
          <h2>Boar Reproductive Identifier Software</h2>
          <p>Lorem ipsum dolor sit amet. Est quia ducimus aut pariatur praesentium et sunt voluptas et aliquam quod et praesentium praesentium. Vel maiores galisum est eaque reprehenderit aut fugit natus ut nulla voluptatem ex doloribus eveniet qui quidem facilis et explicabo rerum. Et nesciunt dolorum aut excepturi omnis ut possimus eligendi aut ullam aspernatur 33 quidem ipsum et saepe reiciendis At cumque nihil. Est quam itaque hic omnis blanditiis et dignissimos quod et corporis obcaecati. </p>
          <p>Est voluptas harum sit repellendus dolores et distinctio assumenda ab omnis fugit nam officia sunt ea repellat distinctio et repellat delectus. Vel vitae iure eum harum aliquam est ducimus sunt et vero nihil. Rem ipsum impedit aut voluptas omnis ad quia dolores in unde quas. Aut officia delectus sed pariatur odio in laborum excepturi ut omnis doloremque aut officia quis. </p>
          <div>
            <img className='logo' src={logoUfg} alt=''/>
            <img className='logo' src={logoEvz} alt=''/>
            <img className='logo' src={logoInf} alt=''/>
          </div>
        </div>
        <FormLogin>
          onLogin={handleLogin}
        </FormLogin>
      </div>
    </div>
  );
};

export default Login;
