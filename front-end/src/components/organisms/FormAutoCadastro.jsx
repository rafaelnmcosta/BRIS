import React from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { MailOutlined, LockOutlined, UserOutlined } from '@ant-design/icons';

const FormAutoCadastro = () => {
  return (
    <div className="flex flex-col items-center justify-center bg-white rounded-3xl shadow-lg py-16 px-20">
      <h2 className="text-green-dark text-3xl font-bold mb-2">CADASTRO</h2>
      <p className="text-green-dark mb-4">
        Já possui cadastro? <span className="text-green-dark">Pode entrar </span><a className="font-bold text-green-dark hover:text-green-light" href="/login">aqui!</a>
      </p>
      <form className="w-full max-w-full">
        <InputSemBordaComLabel 
          label="Nome"
          placeholder="seu nome completo" 
          icone={<UserOutlined className="text-green-dark" />} 
        />

        <InputSemBordaComLabel 
          label="E-mail"
          type="email"
          placeholder="seu e-mail"
          icone={<MailOutlined className="text-green-dark" />} 
        />

        <InputSemBordaComLabel 
          label="Senha"
          type="password"
          placeholder="sua senha"
          icone={<LockOutlined className="text-green-dark" />}
        />

        <InputSemBordaComLabel 
          label="Confirme sua senha"
          type="password"
          placeholder="confirme sua senha"
          icone={<LockOutlined className="text-green-dark" />}
        />

        <div className="flex flex-col items-center justify-center mt-10">
          <BotaoPrimario texto="Cadastrar" />
        </div>
      </form>
    </div>
  );
};

export default FormAutoCadastro;
