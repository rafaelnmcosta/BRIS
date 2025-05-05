import React, { useState } from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import { UserOutlined, MailOutlined, IdcardOutlined, PhoneOutlined, LockOutlined, PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import ModalConfirmacao from './ModalConfirmacao';

const FormEdicaoUsuario = ({
    onSubmit,
    erros,
    onAbrirModal,
    initialData = {},
    vinculos,
    onRemoverVinculo
}) => {
    
    const [vinculoParaRemover, setVinculoParaRemover] = useState(null);
    const [showRemoverVinculo, setShowRemoverVinculo] = useState(false);

    const [formData, setFormData] = React.useState({
        nome: initialData.nome || '',
        email: initialData.email || '',
        cpf: initialData.cpf || '',
        telefone: initialData.telefone || '',
        senha: '',
        confirmarSenha: ''
    });

    React.useEffect(() => {
        if (initialData) {
            setFormData({
                nome: initialData.nome || '',
                email: initialData.email || '',
                cpf: initialData.cpf || '',
                telefone: initialData.telefone || '',
                senha: '',
                confirmarSenha: ''
            });
        }
    }, [initialData]);

    const handleChange = (e) => {
        let { name, value } = e.target;
      
        // remove a máscara de campos específicos
        if (['CPF', 'Telefone'].includes(name)) {
          value = value.replace(/\D/g, '');
        }
      
        setFormData({ ...formData, [name]: value });
      };

    const handleSubmit = (e) => {
        e.preventDefault();

        if (vinculos.length === 0) {
            erros = (prev => ({ ...prev, vinculos: 'É necessário pelo menos um vínculo' }));
            return;
        }

        // Envia os dados atualizados mantendo o ID
        onSubmit({
            ...formData,
            id: initialData.id,
            vinculos
        });
    };

    return (
        <form className="w-full" onSubmit={handleSubmit}>
            <InputSemBordaComLabel
                label="Nome"
                name="nome"
                value={formData.nome}
                onChange={handleChange}
                placeholder="Nome completo"
                icone={<UserOutlined className="text-green-dark" />}
                erro={erros.nome}
            />

            <InputSemBordaComLabel
                label="E-mail"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
                placeholder="exemplo@email.com"
                icone={<MailOutlined className="text-green-dark" />}
                erro={erros.email}
            />

            <InputSemBordaComLabel
                label="CPF"
                name="cpf"
                value={formData.cpf}
                onChange={handleChange}
                placeholder="XXX.XXX.XXX-XX"
                icone={<IdcardOutlined className="text-green-dark" />}
                erro={erros.cpf}
                mask="999.999.999-99"
            />

            <InputSemBordaComLabel
                label="Telefone"
                name="telefone"
                type="tel"
                value={formData.telefone}
                onChange={handleChange}
                placeholder="(XX) XXXXX-XXXX"
                icone={<PhoneOutlined className="text-green-dark" />}
                erro={erros.telefone}
                mask="(99) 99999-9999"
            />

            <InputSemBordaComLabel
                label="Nova Senha (opcional)"
                name="senha"
                type="password"
                value={formData.senha}
                onChange={handleChange}
                placeholder="Deixe em branco para manter a atual"
                icone={<LockOutlined className="text-green-dark" />}
                erro={erros.senha}
            />

            <InputSemBordaComLabel
                label="Confirme a Nova Senha"
                name="confirmarSenha"
                type="password"
                value={formData.confirmarSenha}
                onChange={handleChange}
                placeholder="Repita a nova senha"
                icone={<LockOutlined className="text-green-dark" />}
                erro={erros.confirmarSenha}
            />

            <div className="my-6">
                <div className="flex justify-between items-center mb-4">
                    <h3 className="text-lg font-semibold">Vínculos</h3>
                    <button
                        type="button"
                        onClick={onAbrirModal}
                        className="flex items-center gap-2 text-green-dark hover:text-green"
                    >
                        <PlusOutlined /> Adicionar Vínculo
                    </button>
                </div>

                {vinculos.map((vinculo) => (
                    <div key={vinculo.id} className="p-3 mb-2 border rounded-lg flex justify-between items-center">
                        <div>
                            <p>Perfil: {vinculo.roleId}</p>
                            {vinculo.granjaId && <p>Granja: {vinculo.granjaId}</p>}
                            {vinculo.agroindustriaId && <p>Agroindústria: {vinculo.agroindustriaId}</p>}
                        </div>
                        <button
                            type="button"
                            onClick={() => {
                                setVinculoParaRemover(vinculo.id); // Armazena o ID do vínculo
                                setShowRemoverVinculo(true);
                            }}
                            className="text-red-500 hover:text-red-700 p-1"
                            aria-label="Remover vínculo"
                        >
                            <DeleteOutlined />
                        </button>
                    </div>
                ))}

                <ModalConfirmacao
                    open={showRemoverVinculo}
                    onClose={() => {
                        setShowRemoverVinculo(false);
                        setVinculoParaRemover(null); // Limpa o ID ao cancelar
                    }}
                    onConfirm={() => {
                        onRemoverVinculo(vinculoParaRemover); // Chama a função com o ID armazenado
                        setShowRemoverVinculo(false);
                        setVinculoParaRemover(null);
                    }}
                    title="Confirmar Remoção"
                    content="Tem certeza que deseja remover este vínculo?"
                    okText="Confirmar Remoção"
                    danger={true}
                />
            </div>

            <div className='w-1/2 mx-auto'>
                <BotaoPrimario texto="Salvar Alterações" type="submit" />
            </div>
        </form>
    );
};

export default FormEdicaoUsuario;