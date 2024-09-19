import React, { useState } from 'react';
import axios from 'axios';

import '../App.css';
import './Cadastro.css';

import FormCadastro from '../components/FormCadastro';
import HeadbarLogin from '../components/HeadbarLogin';

const Cadastro = () => {

  return (
    <div>
      <HeadbarLogin/>
        <FormCadastro></FormCadastro>
    </div>
  );
};

export default Cadastro;
