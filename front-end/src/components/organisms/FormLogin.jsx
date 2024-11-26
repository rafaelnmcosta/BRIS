import React, { useState } from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import BotaoPrimario from '../atoms/BotaoPrimario';

const FormLogin = ({ handleLogin }) => {
  const [email, setEmail] = useState('');
  const [senha, setSenha] = useState('');

  const onSubmit = (e) => {
    e.preventDefault();
    handleLogin({ email, senha });
  };

  return (
    <div className="flex flex-col items-center justify-center bg-white rounded-3xl shadow-lg p-16">
      <h2 className="text-green-dark text-3xl font-bold mb-4">LOGIN</h2>
      <form className="w-full max-w-xs" onSubmit={onSubmit}>
        <InputSemBordaComLabel
          label="E-mail"
          placeholder="exemplo@email.com"
          icone={<UserOutlined className="text-green-dark" />}
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />

        <InputSemBordaComLabel
          label="Senha"
          type="password"
          placeholder="******"
          icone={<LockOutlined className="text-green-dark" />}
          value={senha}
          onChange={(e) => setSenha(e.target.value)}
        />

        <div className="flex items-center mt-2 mb-6">
          <input type="checkbox" id="lembrar-me" className="mr-2" />
          <label htmlFor="lembrar-me" className="text-green-dark">Lembrar-me</label>
        </div>

        <div className="flex flex-col items-center justify-center">
          <BotaoPrimario
            texto="Entrar"
            type="submit"

            className="bg-green hover:!bg-green-dark text-white font-bold px-10 rounded-full h-10"
          >
            Entrar
          </BotaoPrimario>
        </div>
      </form>

      <div className="text-center mt-4">
        <a href="/recuperar-senha" className="text-green-dark hover:text-green-light">Esqueceu sua senha?</a>
      </div>
    </div>
  );
};

export default FormLogin;
