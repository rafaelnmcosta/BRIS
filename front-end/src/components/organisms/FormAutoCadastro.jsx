import React from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { MailOutlined, LockOutlined, UserOutlined, PhoneOutlined, IdcardOutlined } from '@ant-design/icons';

const FormAutoCadastro = () => {
  return (
    <div className="bg-white shadow-lg pt-12 px-20 min-h-screen max-h-full flex flex-col items-start">
      <h2 className="text-green-dark text-3xl font-bold mb-4">CADASTRO</h2>
      <form className="w-full">
        <InputSemBordaComLabel
          label="Nome"
          placeholder="Como você prefere ser chamado?"
          icone={<UserOutlined className="text-green-dark" />}
        />

        <InputSemBordaComLabel
          label="E-mail"
          type="email"
          placeholder="exemplo@email.com"
          icone={<MailOutlined className="text-green-dark" />}
        />

        <InputSemBordaComLabel
          label="CPF"
          placeholder="XXX.XXX.XXX-XX"
          icone={<IdcardOutlined className="text-green-dark" />}
        />

        <InputSemBordaComLabel
          label="Telefone"
          type="tel"
          placeholder="(XX) XXXXX-XXXX"
          icone={<PhoneOutlined className="text-green-dark" />}
        />

        <InputSemBordaComLabel
          label="Senha"
          type="password"
          placeholder="******"
          icone={<LockOutlined className="text-green-dark" />}
        />

        <InputSemBordaComLabel
          label="Confirme sua senha"
          type="password"
          placeholder="******"
          icone={<LockOutlined className="text-green-dark" />}
        />

        <div className="my-10">
          <BotaoPrimario
            texto="Cadastrar"
            type="submit"
          />
        </div>
      </form>
    </div>
  );
};

export default FormAutoCadastro;
