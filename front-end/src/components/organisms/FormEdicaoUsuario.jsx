import React from 'react';
import InputSemBordaComLabel from '../molecules/InputSemBordaComLabel';
import BotaoPrimario from '../atoms/BotaoPrimario';
import {
    UserOutlined, MailOutlined, IdcardOutlined,
    PhoneOutlined, LockOutlined, PlusOutlined, DeleteOutlined
} from '@ant-design/icons';
import { useValidation } from '../../services/ValidationContext';
import ModalConfirmacao from './ModalConfirmacao';

const FormEdicaoUsuario = ({ initialData, onSubmit, vinculos, onAbrirModal, onRemoverVinculo }) => {
    const [formData, setFormData] = React.useState({
        nome: '',
        email: '',
        cpf: '',
        telefone: '',
        senha: '',
        confirmarSenha: ''
    });

    const [errors, setErrors] = React.useState({});
    const [vinculoParaRemover, setVinculoParaRemover] = React.useState(null);
    const [showModalRemover, setShowModalRemover] = React.useState(false);

    const {
        validarCampoObrigatorio,
        validarEmail,
        validarCPF,
        validarTelefone,
        validarSenha,
        validarConfirmacaoSenha,
        validarVinculos
    } = useValidation();

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

        if (['cpf', 'telefone'].includes(name)) {
            value = value.replace(/\D/g, '');
        }

        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const validarCampos = () => {
        const novosErros = {};

        const erroNome = validarCampoObrigatorio(formData.nome);
        if (erroNome) novosErros.nome = erroNome;

        const erroEmail = validarCampoObrigatorio(formData.email) || validarEmail(formData.email);
        if (erroEmail) novosErros.email = erroEmail;

        const erroCPF = validarCampoObrigatorio(formData.cpf) || validarCPF(formData.cpf);
        if (erroCPF) novosErros.cpf = erroCPF;

        const erroTelefone = validarTelefone(formData.telefone);
        if (erroTelefone) novosErros.telefone = erroTelefone;

        if (formData.senha) {
            const erroSenha = validarSenha(formData.senha);
            if (erroSenha) novosErros.senha = erroSenha;

            const erroConfirmarSenha = validarConfirmacaoSenha(formData.senha, formData.confirmarSenha);
            if (erroConfirmarSenha) novosErros.confirmarSenha = erroConfirmarSenha;
        }

        const erroVinculos = validarVinculos(vinculos);
        if (erroVinculos) novosErros.vinculos = erroVinculos;

        return novosErros;
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        const errosValidados = validarCampos();
        setErrors(errosValidados);

        if (Object.keys(errosValidados).length > 0) return;

        const dadosParaEnviar = {
            id: initialData.id,
            nome: formData.nome,
            email: formData.email,
            cpf: formData.cpf,
            telefone: formData.telefone,
            senha: formData.senha || null,
            vinculos
        };

        onSubmit(dadosParaEnviar);
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
                error={errors.nome}
            />

            <InputSemBordaComLabel
                label="E-mail"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
                placeholder="exemplo@email.com"
                icone={<MailOutlined className="text-green-dark" />}
                error={errors.email}
            />

            <InputSemBordaComLabel
                label="CPF"
                name="cpf"
                value={formData.cpf}
                onChange={handleChange}
                placeholder="XXX.XXX.XXX-XX"
                icone={<IdcardOutlined className="text-green-dark" />}
                error={errors.cpf}
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
                error={errors.telefone}
                mask="(99) 99999-9999"
            />

            <InputSemBordaComLabel
                label="Nova Senha (opcional)"
                name="senha"
                type="password"
                value={formData.senha}
                onChange={handleChange}
                placeholder="******"
                icone={<LockOutlined className="text-green-dark" />}
                error={errors.senha}
            />

            <InputSemBordaComLabel
                label="Confirme a nova senha"
                name="confirmarSenha"
                type="password"
                value={formData.confirmarSenha}
                onChange={handleChange}
                placeholder="******"
                icone={<LockOutlined className="text-green-dark" />}
                error={errors.confirmarSenha}
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

                {vinculos.map((vinculo, index) => (
                    <div key={index} className="p-3 mb-2 border rounded-lg flex justify-between items-center">
                        <div>
                            <p>Perfil: {vinculo.roleId}</p>
                            {vinculo.granjaId && <p>Granja: {vinculo.granjaId}</p>}
                            {vinculo.agroindustriaId && <p>Agroindústria: {vinculo.agroindustriaId}</p>}
                        </div>
                        <button
                            type="button"
                            onClick={() => {
                                setVinculoParaRemover(vinculo.id);
                                setShowModalRemover(true);
                            }}
                            className="text-red-500 hover:text-red-700 p-1"
                            aria-label="Remover vínculo"
                        >
                            <DeleteOutlined />
                        </button>
                    </div>
                ))}

                {errors.vinculos && <p className="text-red-500 text-sm">{errors.vinculos}</p>}
            </div>

            <ModalConfirmacao
                open={showModalRemover}
                onClose={() => {
                    setVinculoParaRemover(null);
                    setShowModalRemover(false);
                }}
                onConfirm={() => {
                    onRemoverVinculo(vinculoParaRemover);
                    setShowModalRemover(false);
                }}
                title="Confirmar remoção"
                content="Deseja realmente remover este vínculo?"
                okText="Confirmar remoção"
                danger
            />

            <div className="w-1/2 mx-auto">
                <BotaoPrimario texto="Salvar Alterações" type="submit" />
            </div>
        </form>
    );
};

export default FormEdicaoUsuario;
